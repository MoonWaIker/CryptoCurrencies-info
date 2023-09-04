using CryptocurrenciesInfo.Models.Cryptocurrencies;
using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public class CoinMarketRequest : IRequest<IEnumerable<Coin>>
    {
        public int Limit { get; set; }
    }
}