namespace CryptocurrenciesInfo.Utils;

internal static class ExceptionMessages
{
    public const string InvalidId = "ID isn't valid";
    public const string NumberLessThanZero = "Limit must be greater than 0";
    public const string JsonReaderExceptionMessage = "Error while parsing JSON";
    public const string AutOfRangeExceptionMessage = "Page number is out of range";

    public const string ProviderIsNullError =
        "The provider doesn't exist. Make sure it's set in appsettings.json";

    public const string ConnectionStringIsNullError =
        "The connection string doesn't exist. Make sure it's set in appsettings.json";
}