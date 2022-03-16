namespace LiWiMus.SharedKernel;

// ReSharper disable once InconsistentNaming
public static class IEnumerableExtensions
{
    public static void Foreach<T>(this IEnumerable<T> source, Action<T> selector)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (selector == null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        foreach (var e in source)
        {
            selector(e);
        }
    }
}