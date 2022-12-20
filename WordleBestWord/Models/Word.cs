using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace WordleBestWord.Models
{
    /// <summary>
    /// A word that has been split into unicode graphemes for analysis.
    /// </summary>
    [DebuggerDisplay("{value}")]
    public class Word : IEquatable<Word>, IEnumerable<string>
    {
        private readonly string[] graphemeClusters;
        private string value;

        /// <summary>
        /// Create a new Word based on the given string
        /// </summary>
        public Word(string value)
        {
            this.value = value;
            graphemeClusters = GetGraphemeClusters(value).ToArray();
        }

        /// <summary>
        /// Number of grapheme clusters in this word
        /// </summary>
        public int Length => graphemeClusters.Length;

        /// <summary>
        /// Returns the grapheme cluster at the given index
        /// </summary>
        public string this[int index] => graphemeClusters[index];

        /// <summary>
        /// Splits a .net strings into unicode extended grapheme clusters.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetGraphemeClusters(string text)
        {
            var enm = StringInfo.GetTextElementEnumerator(text);
            while (enm.MoveNext())
            {
                yield return enm.GetTextElement();
            }
        }

        public static implicit operator Word(string value) => new(value);

        public static bool operator !=(Word? left, Word? right) => !(left == right);

        public static bool operator ==(Word? left, Word? right)
        {
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>
        /// Copy this word's grapheme clusters to the given Memory<string>
        /// </summary>
        public void CopyGraphemeClusters(Memory<string> destination)
        {
            graphemeClusters.CopyTo(destination);
        }

        public bool Equals(Word? other)
        {
            if (other is null) return false;
            return value.Equals(other.value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Word);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)graphemeClusters).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return graphemeClusters.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString()
        {
            return value;
        }
    }
}