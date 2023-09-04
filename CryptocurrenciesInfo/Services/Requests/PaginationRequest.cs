using CryptocurrenciesInfo.Models.Cryptocurrencies;
using MediatR;

namespace CryptocurrenciesInfo.Services.Requests
{
    public class PaginationRequest : IRequest<PaginatedMarkets>
    {
        public int PageNumber { get; set; }
        public string SearchString { get; set; } = string.Empty;
    }
}