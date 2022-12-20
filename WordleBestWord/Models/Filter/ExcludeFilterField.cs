namespace WordleBestWord.Models.Filter
{
    /// <summary>
    /// Filter field that matches values that are not the excluded constant value
    /// </summary>
    public class ExcludeFilterField : IFilterField
    {
        private readonly string excludeValue;

        /// <summary>
        /// Creates a new ExcludeFilterField
        /// </summary>
        public ExcludeFilterField(string constantValue)
        {
            excludeValue = constantValue;
        }

        public bool IsMatch(string candidate)
        {
            return candidate != excludeValue;
        }

        public override string ToString()
        {
            return $"^{excludeValue}";
        }
    }
}