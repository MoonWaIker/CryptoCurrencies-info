using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Services.Interfaces.Main;

namespace Cryptocurrencies_info.Services.Interfaces.CoinMarket
{
    public interface ICoinMarketBase : IMainInterface
    {
        // Get all coins
        public IEnumerable<Coin> GetCoinMarket();
    }
}