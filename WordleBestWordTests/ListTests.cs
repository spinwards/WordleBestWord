//using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using WordleBestWord.Extensions;
using WordleBestWord.Models;
using WordleBestWord.Models.Filter;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ListTests
    {
        [TestMethod("List destructure '2")]
        public void ListTest2()
        {
            var list = new List<string> { "one", "two" };
            var (one, two) = list;

            Assert.AreEqual(list[0], one);
            Assert.AreEqual(list[1], two);
        }
    }
}