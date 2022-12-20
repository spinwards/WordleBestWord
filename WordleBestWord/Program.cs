using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using WordleBestWord;

var answerList = new Argument<FileInfo>("answerList", "answer list");
var guessList = new Argument<FileInfo>("guessList", "guess list");
var numResults = new Option<int>("-n", () => 20, "number of results to display");
var samplePercent = new Option<double>("-s", () => 1.0, "percent of input files to sample");

var rootCommand = new RootCommand("run wordle best guess finder") { answerList, guessList, numResults, samplePercent };
rootCommand.SetHandler(async (InvocationContext context) =>
    {
        var answerListValue = context.ParseResult.GetValueForArgument(answerList);
        var guessListValue = context.ParseResult.GetValueForArgument(guessList);
        var numResultsValue = context.ParseResult.GetValueForOption(numResults);
        var samplePercentValue = context.ParseResult.GetValueForOption(samplePercent);

        await foreach (var (word, score) in Analysis.Run(
            answerListValue, guessListValue,
            numResultsValue, samplePercentValue,
            context.GetCancellationToken()))
        {
            Console.WriteLine($"{word}\t{score}");
        }
    });

await rootCommand.InvokeAsync(args);