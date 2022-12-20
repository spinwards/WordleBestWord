namespace WordleBestWord.Extensions.Extensions
{
    public static class StatisticsExtensions
    {
        /// <summary>
        /// Interquartile mean is a statistical measure of central tendency based on the truncated mean of the interquartile range.
        /// </summary>
        /// <param name="values">values to summarize</param>
        /// <param name="cutoff">percentage of the values to remove from the begining and end of the list of values</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double IQM(this IEnumerable<int> values, double cutoff = .1)
        {
            var tmp = values.ToArray();
            if (tmp.Length == 0) throw new ArgumentOutOfRangeException(nameof(values));

            var elements = (int)Math.Floor(tmp.Length * cutoff);
            if (tmp.Length - 2 * elements == 0) throw new ArgumentOutOfRangeException(nameof(cutoff)); ;

            Array.Sort(tmp);

            return tmp[elements..^elements].Average();
        }
    }
}