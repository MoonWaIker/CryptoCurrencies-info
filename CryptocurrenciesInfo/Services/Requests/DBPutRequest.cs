using CryptocurrenciesInfo.Models.DataBase;
using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public class DBPutRequest : IRequest
    {
        public CoinGeckoMarket[] CoinGeckoMarkets { get; set; } = Array.Empty<CoinGeckoMarket>();
    }
}