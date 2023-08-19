using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Services.Interfaces.Main;

namespace Cryptocurrencies_info.Services.Interfaces.CoinMarket
{
    public interface ICoinMarketExtended : IMainInterface
    {
        // Get all coins
        public IEnumerable<Coin> GetCoinMarket(int limit);

        // Get coin with markets
        public CoinFull GetCoin(string coinId, CancellationToken cancellationToken);

        // Exchange coins
        public decimal GetExchange(string from, string target, decimal amount);

        // Get an array of coins
        public string[] GetCoinArray();
    }
}