namespace AdventOfCode
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<TSource> IntersectMany<TSource>(this IEnumerable<IEnumerable<TSource>> owner)
        {
            IEnumerable<TSource> intersection = Array.Empty<TSource>();
            owner.ToList().ForEach(s => {
                intersection = !intersection.Any() ? s : s.Intersect(intersection);
            });
            return intersection;
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> owner, Func<T, IEnumerable<T>> selector) => owner.SelectMany(c => selector(c).Flatten(selector)).Concat(owner);
    }


    public static class ReadOnlySpanExtenstion
    {
        public static (int Index, string Content) UniqueItemsInLength<T>(this ReadOnlySpan<T> owner, int chunkLength)
        {
            for (int start = 0; start + 1 + chunkLength <= owner.Length; start++)
            {
                var slice = owner.Slice(start, chunkLength);
                if (!slice
                    .ToArray()
                    .GroupBy(a => a)
                    .SelectMany(ab => ab.Skip(1).Take(1))
                    .ToList()
                    .Any())
                {
                    return (start + chunkLength, slice.ToString());
                }
            }
            return (-1, string.Empty);
        }
    }


    public static class ConsoleExtension
    {
        public static void WriteNewLine()
        {
            Console.WriteLine(Environment.NewLine);
        }
    }
}
