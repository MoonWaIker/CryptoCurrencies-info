using Cryptocurrencies_info.Models.DataBase;
using MediatR;

namespace Cryptocurrencies_info.Services.Requests
{
    public class RequestFromDB : IRequest<IEnumerable<CoinGeckoMarket>>
    {
        public required IEnumerable<MarketBase> Markets { get; set; }
    }
}