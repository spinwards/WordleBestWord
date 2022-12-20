using System.Diagnostics.CodeAnalysis;
using WordleBestWord.Models.Filter;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MatchFieldTests
    {
        private static string[] alphas = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(c => c.ToString()).ToArray();
        private static string[] combining = new[] { "ā̈", "ç" };
        private static string[] all = alphas.Concat(combining).ToArray();

        [TestMethod("Wildcard match field")]
        public void MatchFieldTest1()
        {
            var hole = new WildcardFilterField();
            Assert.IsTrue(all.All(hole.IsMatch));
        }

        [TestMethod("Constant match field")]
        public void MatchFieldTest2()
        {
            foreach (var candidate in all)
            {
                var hole = new ExactFilterField(candidate);
                Assert.AreEqual(1, all.Count(hole.IsMatch));
            }
        }

        [TestMethod("Exclude match field")]
        public void MatchFieldTest3()
        {
            foreach (var candidate in all)
            {
                var hole = new ExcludeFilterField(candidate);
                Assert.AreEqual(all.Length - 1, all.Count(hole.IsMatch));
            }
        }

    }
}