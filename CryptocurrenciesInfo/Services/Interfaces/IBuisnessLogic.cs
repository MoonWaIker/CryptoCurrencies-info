using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Services.Interfaces.Main;

namespace Cryptocurrencies_info.Services.Interfaces
{
    public interface IBuisnessLogic : IMainInterface
    {
        public Task<PaginatedMarkets> Pagination(int pageNumber, string searchString, CancellationToken cancellationToken);
    }
}