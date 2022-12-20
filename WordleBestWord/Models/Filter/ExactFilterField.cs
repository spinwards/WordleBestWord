namespace WordleBestWord.Models.Filter
{
    /// <summary>
    /// Filter field that matches a constant value
    /// </summary>
    public class ExactFilterField : IFilterField
    {
        private readonly string constantValue;

        /// <summary>
        /// Create a new ConstantFilterField
        /// </summary>
        public ExactFilterField(string constantValue)
        {
            this.constantValue = constantValue;
        }

        public bool IsMatch(string candidate)
        {
            return candidate == constantValue;
        }

        public override string ToString()
        {
            return constantValue;
        }
    }
}