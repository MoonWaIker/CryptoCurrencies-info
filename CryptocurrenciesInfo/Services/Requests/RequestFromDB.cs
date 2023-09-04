using CryptocurrenciesInfo.Models.DataBase;
using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public class RequestFromDB : IRequest<IEnumerable<CoinGeckoMarket>>
    {
        public required IEnumerable<MarketBase> Markets { get; set; }
    }
}