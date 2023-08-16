using Cryptocurrencies_info.Models.Cryptocurrencies;

namespace Cryptocurrencies_info.Services.Interfaces.CoinMarket
{
    public interface ICoinMarketBase
    {
        // Get all coins
        public IEnumerable<Coin> GetCoinMarket();
    }

}