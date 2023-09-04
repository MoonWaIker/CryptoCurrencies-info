using CryptocurrenciesInfo.Models.Cryptocurrencies;
using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public class CoinRequest : IRequest<CoinFull>
    {
        public string Id { get; set; } = string.Empty;
    }
}