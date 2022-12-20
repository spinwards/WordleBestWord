using System.Buffers;
using System.Diagnostics;

namespace WordleBestWord.Models.Filter
{
    /// <summary>
    /// A filter composed of IFilterField created by scoring a reference value against a
    /// candidate using simplified rules from the game Wordle.
    /// </summary>
    [DebuggerDisplay("{Description}")]
    public class FilterExpression
    {
        private readonly IFilterField[] fields;

        /// <summary>
        /// Create a new filter expression
        /// </summary>
        /// <param name="referenceWord">A word from the set of solutions</param>
        /// <param name="candidateWord">A word from the set of valid guess words</param>
        /// <exception cref="ArgumentException"></exception>
        public FilterExpression(Word referenceWord, Word candidateWord)
        {
            if (referenceWord.Length != candidateWord.Length)
                throw new ArgumentException("Candidate word must be the same length as reference word", nameof(candidateWord));

            // Populate the expression with wildcards that match any grapheme cluster
            fields = new IFilterField[referenceWord.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = new WildcardFilterField();
            }

            // copy the reference word to working memory so that it can be modified as matches are found
            string[] reference = new string[referenceWord.Length];
            referenceWord.CopyGraphemeClusters(reference.AsMemory());

            // Scan for green squares first so that they are not considered when searching for yellow squares
            for (var i = 0; i < reference.Length; i++)
            {
                if (reference[i] == candidateWord[i])
                {
                    fields[i] = new ExactFilterField(reference[i]);
                    reference[i] = string.Empty;
                }
            }

            // scan for yellow squares and remove matches from the reference so that repeated
            // letters in the candidate do not match the same letter in the reference
            for (var i = 0; i < reference.Length; i++)
            {
                var candidate = candidateWord[i];
                var ix = Array.IndexOf(reference, candidate);
                if (ix >= 0)
                {
                    fields[i] = new ExcludeFilterField(candidate);
                    reference[ix] = string.Empty;
                }
            }

            Description = $"{string.Join("", referenceWord)}+{string.Join("", candidateWord)}={{{string.Join(",", fields.Select(f => f.ToString()))}}}";
        }

        /// <summary>
        /// A description of this filter
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Returns true if the candidate matches this filter expression, false otherwise
        /// </summary>
        public bool IsMatch(Word candidate)
        {
            for (var i = 0; i < candidate.Length; i++)
            {
                if (!fields[i].IsMatch(candidate[i]))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}