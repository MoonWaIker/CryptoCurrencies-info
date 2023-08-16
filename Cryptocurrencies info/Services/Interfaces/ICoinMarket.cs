using Cryptocurrencies_info.Models.Cryptocurrencies;

namespace Cryptocurrencies_info.Services.Interfaces
{
    public interface ICoinMarket
    {
        // Get all coins
        public IEnumerable<Coin> GetCoinMarket();
        public IEnumerable<Coin> GetCoinMarket(int limit);

        // Get coin with markets
        public CoinFull GetCoin(string coinId);

        // Exchange coins
        public decimal GetExchange(string from, string target, decimal amount);

        // Get an array of coins
        public string[] GetCoinArray();
    }

}