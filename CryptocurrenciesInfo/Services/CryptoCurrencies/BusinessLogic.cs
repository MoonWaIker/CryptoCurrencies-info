using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Services.Interfaces;
using CryptocurrenciesInfo.Services.Interfaces.Main;
using CryptocurrenciesInfo.Services.Requests;
using CryptocurrenciesInfo.Utils;

namespace CryptocurrenciesInfo.Services.CryptoCurrencies;

public sealed class BusinessLogic(IHandler mediator) : IBusinessLogic
{
    private const int Size = 100;

    public async Task<PaginatedMarkets> Pagination(int pageNumber, string searchString,
                                                   CancellationToken cancellationToken)
    {
        CoinMarketRequest request = new();
        var coins = (await mediator.Handle(request, cancellationToken))
            .ToArray();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            coins = coins
                    .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString))
                    .ToArray();
        }

        return pageNumber >= 0 && pageNumber <= coins.Length / Size
            ? new PaginatedMarkets
            {
                Data = coins
                       .Skip(Size * pageNumber)
                       .Take(Size),
                PageNumber = pageNumber,
                MaxPages = coins.Length / Size - 1,
                SearchString = searchString,
                Size = Size
            }
            : throw new ArgumentOutOfRangeException(nameof(pageNumber), ExceptionMessages.AutOfRangeExceptionMessage);
    }
}