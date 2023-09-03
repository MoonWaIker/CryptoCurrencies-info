using MediatR;

namespace Cryptocurrencies_info.Services.Requests
{
    public class CoinArrayRequest : IRequest<string[]> { }
}