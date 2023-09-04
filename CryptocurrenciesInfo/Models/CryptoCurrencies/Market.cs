using CryptocurrenciesInfo.Models.DataBase;

namespace CryptocurrenciesInfo.Models.Cryptocurrencies
{
    public class Market : CoinGeckoMarket
    {
        public decimal Price { get; set; }
    }
}