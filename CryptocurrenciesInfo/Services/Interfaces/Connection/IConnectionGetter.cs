using CryptocurrenciesInfo.Models.DataBase;

namespace CryptocurrenciesInfo.Services.Interfaces.Connection
{
    public interface IConnectionGetter
    {

        // Read and return data from sql
        public IEnumerable<CoinGeckoMarket> GetMarkets(IEnumerable<MarketBase> markets);
    }
}