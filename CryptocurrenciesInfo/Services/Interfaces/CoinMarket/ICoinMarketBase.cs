using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Services.Interfaces.Main;

namespace CryptocurrenciesInfo.Services.Interfaces.CoinMarket
{
    public interface ICoinMarketBase : IMainInterface
    {
        // Get all coins
        public IEnumerable<Coin> GetCoinMarket();
    }
}