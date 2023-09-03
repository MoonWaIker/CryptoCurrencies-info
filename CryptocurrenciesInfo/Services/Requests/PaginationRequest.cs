using Cryptocurrencies_info.Models.Cryptocurrencies;
using MediatR;

namespace Cryptocurrencies_info.Services.Requests
{
    public class PaginationRequest : IRequest<PaginatedMarkets>
    {
        public int PageNumber { get; set; }
        public string SearchString { get; set; } = string.Empty;
    }
}