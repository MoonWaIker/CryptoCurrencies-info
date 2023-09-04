using CryptocurrenciesInfo.Models.DataBase;

namespace CryptocurrenciesInfo.Services.Interfaces.Connection
{
    public interface IConnectionFiller
    {
        // Add markets to sql
        public void AddMarkets(IEnumerable<CoinGeckoMarket> markets);
    }
}