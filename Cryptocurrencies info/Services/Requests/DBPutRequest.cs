using Cryptocurrencies_info.Models.DataBase;
using MediatR;

namespace Cryptocurrencies_info.Services.Requests
{
    public class DBPutRequest : IRequest
    {
        public CoinGeckoMarket[] CoinGeckoMarkets { get; set; } = Array.Empty<CoinGeckoMarket>();
    }
}