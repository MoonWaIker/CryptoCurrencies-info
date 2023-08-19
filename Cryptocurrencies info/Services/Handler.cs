using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces;
using Cryptocurrencies_info.Services.Interfaces.CoinMarket;
using Cryptocurrencies_info.Services.Interfaces.Connection;
using Cryptocurrencies_info.Services.Interfaces.Main;
using Cryptocurrencies_info.Services.Requests;
using MediatR;

namespace Cryptocurrencies_info.Services
{
    public sealed class Handler : IHandler
    {
        // Objects
        private readonly ICoinMarketExtended coinMarketExtanded;
        private readonly ICoinMarketBase coinMarketBase;
        private readonly IBuisnessLogic buisnessLogic;
        private readonly IConnectionGetter connectionGetter;
        private readonly IConnectionFiller connectionFiller;

        // Constructor
        public Handler(IServiceProvider serviceProvider)
        {
            coinMarketExtanded = serviceProvider.GetRequiredService<ICoinMarketExtended>();
            coinMarketExtanded.Mediator = this;
            coinMarketBase = serviceProvider.GetRequiredService<ICoinMarketBase>();
            coinMarketBase.Mediator = this;
            buisnessLogic = serviceProvider.GetRequiredService<IBuisnessLogic>();
            buisnessLogic.Mediator = this;
            connectionGetter = serviceProvider.GetRequiredService<IConnectionGetter>();
            connectionFiller = serviceProvider.GetRequiredService<IConnectionFiller>();
        }

        // Mediator tasks
        Task<PaginatedMarkets> IRequestHandler<PaginationRequest, PaginatedMarkets>.Handle(PaginationRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(buisnessLogic.Pagination(request.PageNumber, request.SearchString, cancellationToken));
        }

        Task<IEnumerable<Coin>> IRequestHandler<CoinMarketRequest, IEnumerable<Coin>>.Handle(CoinMarketRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.Limit == 0 ? coinMarketBase.GetCoinMarket() : coinMarketExtanded.GetCoinMarket(request.Limit));
        }

        Task<string[]> IRequestHandler<CoinArrayRequest, string[]>.Handle(CoinArrayRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(coinMarketExtanded.GetCoinArray());
        }

        Task<CoinFull> IRequestHandler<CoinRequest, CoinFull>.Handle(CoinRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(coinMarketExtanded.GetCoin(request.Id, cancellationToken));
        }

        Task IRequestHandler<DBPutRequest>.Handle(DBPutRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() => connectionFiller.AddMarkets(request.CoinGeckoMarkets), cancellationToken);
        }

        Task<IEnumerable<CoinGeckoMarket>> IRequestHandler<RequestFromDB, IEnumerable<CoinGeckoMarket>>.Handle(RequestFromDB request, CancellationToken cancellationToken)
        {
            return Task.FromResult(connectionGetter.GetMarkets(request.Markets));
        }
    }
}