using Cryptocurrencies_info.Models.Cryptocurrencies;
using MediatR;

namespace Cryptocurrencies_info.Services.Requests
{
    public class CoinRequest : IRequest<CoinFull>
    {
        public string Id { get; set; } = string.Empty;
    }
}