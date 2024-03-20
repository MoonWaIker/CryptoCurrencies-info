using CryptocurrenciesInfo.Models.Cryptocurrencies;

namespace CryptocurrenciesInfo.Services.Interfaces;

internal interface IBusinessLogic
{
    public Task<PaginatedMarkets> Pagination(int pageNumber, string searchString, CancellationToken cancellationToken);
}