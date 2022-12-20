using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks.Dataflow;
using WordleBestWord.Extensions;
using WordleBestWord.Extensions.Extensions;
using WordleBestWord.Models;
using WordleBestWord.Models.Filter;

namespace WordleBestWord
{
    [ExcludeFromCodeCoverage(Justification = "Records do not need to be unit tested")]
    public readonly record struct ScoredWord(string Word, double Score);

    /// <summary>
    /// The game of wordle consists of a hidden word that the user must guess. Solutions and guesses must be the same length.
    /// Each guess is scored using a series of colored blocks, one for each letter. For each position, if the
    /// letter in the solution matches the guess, a green block is displayed. If the letter in the guess is
    /// contained in the solution, but not in the current position a yellow block is displayed. If the letter
    /// in the guess is not contained in the solution, the block remains grey.
    ///
    /// The first guess is effectivly random because the user is not given information about the game's state
    /// until the first guess is scored.
    ///
    /// To efficiently solve the puzzle, each guess should match the scores of each previous guess. If the number of
    /// possible guess words is fixed, then each guess and score reducess the number of valid guesses from the set
    /// of all guesses.
    ///
    /// This analysis uses a simplified version of the wordle scoring mechanism to calculate the number of guess
    /// words that are valid after the opening guess for a given solution and guess word. This analysis is repeated
    /// for each guess for each solution. The scores are then summarized across all solution words for
    /// each guess word. The word with the lowest score can be considered the best opening word
    ///
    /// To speed up the search for the best opening word, this analysis only considers candidate words
    /// that are also possible solitions and are composed of unique letters. To make the summary more robust
    /// the interquartile mean is used instead of the average.
    /// </summary>
    public static class Analysis
    {
        /// <summary>
        /// Run the analysis
        /// </summary>
        /// <param name="solutionList">File containing the set of solutions</param>
        /// <param name="candidateList">File containing the set of valid gueses</param>
        /// <param name="numResults">The number of results to report</param>
        /// <param name="samplePercent">The percent of words to sample from each file</param>
        public static async IAsyncEnumerable<ScoredWord> Run(
            FileInfo solutionList, FileInfo candidateList,
            int numResults = 20, double samplePercent = 1.0,
            [EnumeratorCancellation] CancellationToken cancellation = default)
        {
            var (solutions, candidates) = await Task.WhenAll(
                ReadFile(solutionList.FullName, samplePercent, cancellation).ToArrayAsync(cancellation).AsTask(),
                ReadFile(candidateList.FullName, samplePercent, cancellation).ToArrayAsync(cancellation).AsTask());

            if (cancellation.IsCancellationRequested)
                yield break;

            // Process scores in parallel using a fan-out concurrency pattern
            // managed by the TPL data flow framework

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            var executionOptions = new ExecutionDataflowBlockOptions
            {
                // This is a CPU bound operation and can slam the host CPU
                // so reserve a little bit of capacity for other threads and processes
                MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount - 2)
            };

            // Input buffer to decouple the input from downstream processing
            var input = new BufferBlock<WordBatch>(executionOptions);

            // Perform the calculation
            var output = new TransformBlock<WordBatch, (Word Candidate, double Average)>(
                batch => (batch.Candidate, batch
                        .Select(solution => new FilterExpression(solution, batch.Candidate))
                        .Select(filter => candidates.Count(filter.IsMatch))
                        .IQM(.2)), executionOptions);

            using (input.LinkTo(output, linkOptions))
            {
                // Populate the work queue on a background thread
                _ = Task.Run(() => PopulateWorkQueue(solutions, input, cancellation), cancellation);

                await foreach (var (Candidate, Average) in output
                    .ReceiveAllAsync(cancellation)
                    .OrderBy(m => m.Average)
                    .Take(numResults)
                    .ConfigureAwait(false))
                {
                    yield return new ScoredWord { Word = Candidate.ToString(), Score = Average };
                }
            }
        }

        /// <summary>
        /// Populate the work queue with candidate words from the solution list that are composed of unique letters
        /// </summary>
        /// <param name="solutions">game solutions</param>
        /// <param name="input">input work queue</param>
        /// <param name="cancellation">the cancellation token used to cancel this operation</param>
        private static async Task PopulateWorkQueue(
            Word[] solutions,
            BufferBlock<WordBatch> input,
            CancellationToken cancellation)
        {
            var interestingCandidates = solutions.Where(UniqueGraphemeClusters).ToArray();
            for (int i = 0; i < interestingCandidates.Length; i++)
            {
                var candidate = interestingCandidates[i];
                var batch = new WordBatch(candidate);

                for (int j = 0; j < interestingCandidates.Length; j++)
                {
                    if (cancellation.IsCancellationRequested) break;

                    var solution = solutions[j];
                    if (solution == candidate) continue;

                    batch.Add(solution);
                }
                if (cancellation.IsCancellationRequested) break;
                await input.SendAsync(batch, cancellation);
            }
            input.Complete();
        }

        /// <summary>
        /// Returns true if each grapheme cluster in the word is used exactly one time
        /// </summary>
        public static bool UniqueGraphemeClusters(Word str)
        {
            for (int i = 0; i < str.Length - 1; i++)
            {
                for (int j = i + 1; j < str.Length; j++)
                {
                    if (str[i] == str[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Read the given file and parse each line into Word objects optionally sampling a subset of the lines in the file
        /// </summary>
        /// <param name="fileName">file to load</param>
        /// <param name="samplePercent">percentage of lines to load sampled at random</param>
        /// <param name="cancellation">cancellation token to cancel the operation</param>
        /// <returns></returns>
        public static async IAsyncEnumerable<Word> ReadFile(
            string fileName, double samplePercent = 1.0,
            [EnumeratorCancellation] CancellationToken cancellation = default)
        {
            await foreach (var line in File.ReadLinesAsync(fileName, cancellation))
            {
                if (cancellation.IsCancellationRequested)
                    yield break;

                var tmp = line.Trim();
                if (tmp.Length > 0 && tmp[0] != '#'
                    && (samplePercent == 1.0 || Random.Shared.NextDouble() < samplePercent))
                {
                    yield return new Word(tmp);
                }
            }
        }

        // Utility class that groups batches of work
        private class WordBatch : List<Word>
        {
            public WordBatch(Word guess) : base() => Candidate = guess;

            public Word Candidate { get; }
        }
    }
}