namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class PaginatedMarkets
    {
        public IEnumerable<Coin> Data { get; set; } = Array.Empty<Coin>();

        public int PageNumber { get; set; }

        public int MaxPages { get; set; }

        public string SearchString { get; set; } = string.Empty;

        public int Size { get; set; }
    }
}