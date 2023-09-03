using Cryptocurrencies_info.Models.Cryptocurrencies;
using MediatR;

namespace Cryptocurrencies_info.Services.Requests
{
    public class CoinMarketRequest : IRequest<IEnumerable<Coin>>
    {
        public int Limit { get; set; }
    }
}