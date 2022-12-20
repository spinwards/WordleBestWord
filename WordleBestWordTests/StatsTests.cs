using System.Diagnostics.CodeAnalysis;
using WordleBestWord.Extensions.Extensions;
using WordleBestWord.Models;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StatsTests
    {
        [TestMethod("IQM IEnumerable<int>")]
        public void StatsTest1()
        {
            var iqm = Enumerable.Range(0, 100).IQM();

            Assert.AreEqual(49.5, iqm);
        }
    }
}