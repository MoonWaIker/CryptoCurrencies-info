using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Services.Interfaces.Main;

namespace CryptocurrenciesInfo.Services.Interfaces
{
    public interface IBuisnessLogic : IMainInterface
    {
        public Task<PaginatedMarkets> Pagination(int pageNumber, string searchString, CancellationToken cancellationToken);
    }
}