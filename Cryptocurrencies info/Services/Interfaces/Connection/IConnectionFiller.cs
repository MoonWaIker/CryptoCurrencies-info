using Cryptocurrencies_info.Models.DataBase;

namespace Cryptocurrencies_info.Services.Interfaces.Connection
{
    public interface IConnectionFiller
    {
        // Add markets to sql
        public void AddMarkets(CoinGeckoMarket[] markets);
    }
}