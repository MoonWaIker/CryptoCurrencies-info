using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public class CoinArrayRequest : IRequest<string[]> { }
}