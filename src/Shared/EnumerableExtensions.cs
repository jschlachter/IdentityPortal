namespace System.Collections.Generic;

internal static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T>? collection)
    {
        return collection?.Any() != true;
    }
}
