using Cryptocurrencies_info.Models.DataBase;

namespace Cryptocurrencies_info.Services.Interfaces.Connection
{
    public interface IConnectionFiller
    {
        // Add markets to sql
        // TODO redo passing type to IEnumerable, when redo MicrosoftSql provider
        public void AddMarkets(CoinGeckoMarket[] markets);
    }
}