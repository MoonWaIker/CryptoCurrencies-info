using CryptocurrenciesInfo.Models.Cryptocurrencies;
using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public sealed class CoinMarketRequest : IRequest<IEnumerable<Coin>>
    {
        internal int Limit { get; init; }
    }
}