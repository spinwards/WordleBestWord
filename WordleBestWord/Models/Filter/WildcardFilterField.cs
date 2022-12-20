namespace WordleBestWord.Models.Filter
{
    /// <summary>
    /// A filter field that matches any value
    /// </summary>
    public class WildcardFilterField : IFilterField
    {
        public bool IsMatch(string candidate)
        {
            return true;
        }

        public override string ToString()
        {
            return "*";
        }
    }
}