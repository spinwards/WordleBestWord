//using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using WordleBestWord;
using WordleBestWord.Models;
using WordleBestWord.Models.Filter;


namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FilterTests
    {
        private static string[] alphas = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(c => c.ToString()).ToArray();

        [TestMethod("Candidate filter no matches test")]
        public void FilterTest1()
        {
            var samples = alphas.Chunk(5).Select(s=>new Word(string.Join("", s))).ToList();
            var filtered = samples.Where(Analysis.UniqueGraphemeClusters).ToList();
            CollectionAssert.AreEquivalent(samples, filtered);
        }

        [TestMethod("Candidate filter matches removed test")]
        public void FilterTest2()
        {
            var samples = alphas.Chunk(5).Select(s => new Word(string.Join("", s))).ToList();
            var filteredCount = samples.Count;

            samples.Insert(Random.Shared.Next(1, samples.Count), "aabcd");
            samples.Insert(Random.Shared.Next(1, samples.Count), "abbcd");
            samples.Insert(Random.Shared.Next(1, samples.Count), "aabbc");


            var filtered = samples.Where(Analysis.UniqueGraphemeClusters).ToList();
            Assert.AreEqual(filteredCount, filtered.Count);
        }
    }
}