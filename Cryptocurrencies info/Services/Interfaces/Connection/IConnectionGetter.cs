using Cryptocurrencies_info.Models.DataBase;

namespace Cryptocurrencies_info.Services.Interfaces.Connection
{
    public interface IConnectionGetter
    {

        // Read and return data from sql
        public IEnumerable<CoinGeckoMarket> GetMarkets(IEnumerable<MarketBase> markets);
    }
}