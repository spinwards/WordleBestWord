namespace WordleBestWord.Models.Filter
{
    /// <summary>
    /// A field in a filter expression that matches a single grapheme cluster
    /// </summary>
    public interface IFilterField
    {
        /// <summary>
        /// Returns true if the value matches the filter, false otherwise
        /// </summary>
        bool IsMatch(string value);
    }
}