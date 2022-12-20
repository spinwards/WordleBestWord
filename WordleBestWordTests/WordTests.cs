//using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using WordleBestWord.Models;

namespace WordleBestWordTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WordTests
    {
        private static string[] alphas = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Select(c => c.ToString()).ToArray();

        [TestMethod("Word equality")]
        public void WordTest1()
        {
            Assert.IsTrue(new Word("test").Equals(new Word("test")));
        }

        [TestMethod("Word equals operator")]
        public void WordTest2()
        {
            Assert.IsTrue(new Word("test") == new Word("test"));
        }

        [TestMethod("Word not equals operator")]
        public void WordTest3()
        {
            Assert.IsTrue(new Word("test") != new Word("toast"));
        }

        [TestMethod("Word copy tokens")]
        public void WordTest4()
        {
            var word = new Word("test");

            var dest = new string[word.Length];
            word.CopyGraphemeClusters(dest);

            CollectionAssert.AreEquivalent(new List<string>(word), dest);
        }

        [TestMethod("Word copy tokens with IEnumerable")]
        public void WordTest5()
        {
            var word = new Word("test");

            var dest = new string[word.Length];
            word.CopyGraphemeClusters(dest);

            var tokens = new ArrayList();
            var enm = ((IEnumerable)word).GetEnumerator();
            while(enm.MoveNext())
            {
                tokens.Add(enm.Current);
            }

            CollectionAssert.AreEquivalent(tokens, dest);
        }

        [TestMethod("Word equality")]
        public void WordTest6()
        {
            var word = new Word("test");
            var word2 = new Word("test");
            var word3 = new Word("tttt");

            Assert.IsTrue(word == word2);
            Assert.IsTrue(word.Equals(word2));

            Assert.IsFalse(word == word3);
            Assert.IsFalse(word.Equals(word3));
        }

        [TestMethod("Word not equals")]
        public void WordTest7()
        {
            var word = new Word("test");
            var word2 = new Word("test");
            var word3 = new Word("tttt");

            Assert.IsTrue(word != word3);
            Assert.IsFalse(word != word2);
            
        }
    }
}