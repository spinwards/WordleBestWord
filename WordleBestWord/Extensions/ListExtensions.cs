namespace WordleBestWord.Extensions
{
    public static class ListExtensions
    {
        public static void Deconstruct<T>(this IList<T> list, out T first, out T second)
        {
            first = list.Count > 0 ? list[0] : throw new ArgumentException("Not enough items to deconstruct", nameof(list));
            second = list.Count > 1 ? list[1] : throw new ArgumentException("Not enough items to deconstruct", nameof(list));
        }
    }
}