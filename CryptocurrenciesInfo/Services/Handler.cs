using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Services.Interfaces;
using CryptocurrenciesInfo.Services.Interfaces.CoinMarket;
using CryptocurrenciesInfo.Services.Interfaces.Main;
using CryptocurrenciesInfo.Services.Requests;

namespace CryptocurrenciesInfo.Services;

public sealed class Handler(IServiceProvider serviceProvider) : IHandler
{
    public Task<PaginatedMarkets> Handle(PaginationRequest request, CancellationToken cancellationToken)
    {
        var businessLogic = serviceProvider.GetRequiredService<IBusinessLogic>();

        return Task.Run(
            () => businessLogic.Pagination(request.PageNumber, request.SearchString, cancellationToken),
            cancellationToken);
    }

    public Task<IEnumerable<Coin>> Handle(CoinMarketRequest request, CancellationToken cancellationToken)
    {
        var coinMarketExtended = serviceProvider.GetRequiredService<ICoinMarket>();

        return Task.Run(() => request.Limit == 0
                            ? coinMarketExtended.GetCoinMarket()
                            : coinMarketExtended.GetCoinMarket(request.Limit), cancellationToken);
    }

    public Task<IEnumerable<string>> Handle(CoinArrayRequest request, CancellationToken cancellationToken)
    {
        var coinMarketExtended = serviceProvider.GetRequiredService<ICoinMarket>();

        return Task.Run(() => coinMarketExtended.GetCoinArray(), cancellationToken);
    }

    public Task<CoinFull> Handle(CoinRequest request, CancellationToken cancellationToken)
    {
        var coinMarketExtended = serviceProvider.GetRequiredService<ICoinMarket>();

        return Task.Run(() => coinMarketExtended.GetCoin(request.Id), cancellationToken);
    }
}