//using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using WordleBestWord.Models;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TokenizerTests
    {
        [TestMethod("Tokenizer single char words")]
        public void TokenizerTest1()
        {
            var reference = "SingleCharWord";
            var referenceTokens = reference.Select(c => c.ToString()).ToArray();

            var tokens = Word.GetGraphemeClusters(reference).ToArray();

            CollectionAssert.AreEqual(referenceTokens, tokens);
        }

        [TestMethod("Tokenizer multi char words")]
        public void TokenizerTest2()
        {
            var reference = "a\u0304\u0308bc\u0327";
            var referenceTokens = new[] { "ā̈", "b", "ç" };

            var tokens = Word.GetGraphemeClusters(reference).ToArray();

            CollectionAssert.AreEqual(referenceTokens, tokens);
        }
    }
}