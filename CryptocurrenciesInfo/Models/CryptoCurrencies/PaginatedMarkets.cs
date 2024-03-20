namespace CryptocurrenciesInfo.Models.Cryptocurrencies
{
    public sealed class PaginatedMarkets
    {
        public IEnumerable<Coin> Data { get; init; } = Array.Empty<Coin>();

        public int PageNumber { get; init; }

        public int MaxPages { get; init; }

        public string SearchString { get; init; } = string.Empty;

        public int Size { get; init; }
    }
}