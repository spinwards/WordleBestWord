//using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using WordleBestWord;
using WordleBestWord.Models;
using WordleBestWord.Models.Filter;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FileTests
    {
        private static string[] alphas = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(c => c.ToString()).ToArray();

        [TestMethod("Read words from file")]
        public async Task FileTest1()
        {
            var file = Path.GetTempFileName();
            try
            {
                var samples = alphas.Chunk(5).Select(s => new Word(string.Join("", s))).ToArray();

                File.WriteAllLines(file, samples.Select(w => w.ToString()));

                var fromDisk = await Analysis.ReadFile(file).ToArrayAsync();

                CollectionAssert.AreEquivalent(samples, fromDisk);
            }
            finally
            {
                File.Delete(file);
            }
        }
    }
}