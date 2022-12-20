using System.Diagnostics.CodeAnalysis;
using WordleBestWord.Models;
using WordleBestWord.Models.Filter;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MatchExpressionTests
    {
        [TestMethod("Word filter construction")]
        public void MatchExpressionTest1()
        {
            var filter = new FilterExpression("prune", "crane");

            Assert.AreEqual("prune+crane={*,r,*,n,e}", filter.ToString());
            Assert.AreEqual("prune+crane={*,r,*,n,e}", filter.Description);
        }

        [TestMethod("Word filter constructor exception")]
        public void MatchExpressionTest2()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                var filter = new FilterExpression("prune", "craned");
            });
        }

        [TestMethod("Word filter match")]
        public void MatchExpressionTest3()
        {
            var filter = new FilterExpression("prune", "crane");
            Assert.IsTrue(filter.IsMatch("brine"));
        }

        [TestMethod("Word filter not match")]
        public void MatchExpressionTest4()
        {
            var filter = new FilterExpression("prune", "crane");
            Assert.IsFalse(filter.IsMatch("brain"));
        }

        [TestMethod("Word filter match count")]
        public void MatchExpressionTest5()
        {
            var filter = new FilterExpression("prune", "ruins");

            var candidates = new Word[] {
                // Not match
                "brain",
                "brine",
                "unite",
                "untie",

                // Match
                "print",
                "prune",
                "ruins"
            };

            Assert.AreEqual(3, candidates.Count(filter.IsMatch));
        }
    }
}