using System.Diagnostics;

namespace System.Text;

internal static class StringExtensions
{
    [DebuggerStepThrough]
    public static bool IsPresent(this string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    [DebuggerStepThrough]
    public static bool IsMissing(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}
