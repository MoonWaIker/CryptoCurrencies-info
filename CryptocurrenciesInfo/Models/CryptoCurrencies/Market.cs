using Cryptocurrencies_info.Models.DataBase;

namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class Market : CoinGeckoMarket
    {
        public decimal Price { get; set; }
    }
}