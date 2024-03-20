namespace CryptocurrenciesInfo.Utils;

internal static class StringExtensions
{
    public static bool IsValid(this string? str)
    {
        return !string.IsNullOrWhiteSpace(str);
    }
}