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
        // TODO connection to db was replaced by service provider (mean at new query make new object), cause EF can add and update same objects once. But is possible other way?
        // TODO can connection interfaces be as single?
        private readonly IServiceProvider serviceProvider;
        private readonly ICoinMarketExtended coinMarketExtanded;
        private readonly ICoinMarketBase coinMarketBase;
        private readonly IBuisnessLogic buisnessLogic;

        // Constructor
        public Handler(IServiceProvider serviceProvider)
        {
            coinMarketExtanded = serviceProvider.GetRequiredService<ICoinMarketExtended>();
            coinMarketExtanded.Mediator = this;
            coinMarketBase = serviceProvider.GetRequiredService<ICoinMarketBase>();
            coinMarketBase.Mediator = this;
            buisnessLogic = serviceProvider.GetRequiredService<IBuisnessLogic>();
            buisnessLogic.Mediator = this;
            this.serviceProvider = serviceProvider;
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
            IConnectionFiller connectionFiller = serviceProvider.GetRequiredService<IConnectionFiller>();
            return Task.Run(() => connectionFiller.AddMarkets(request.CoinGeckoMarkets), cancellationToken);
        }

        Task<IEnumerable<CoinGeckoMarket>> IRequestHandler<RequestFromDB, IEnumerable<CoinGeckoMarket>>.Handle(RequestFromDB request, CancellationToken cancellationToken)
        {
            IConnectionGetter connectionGetter = serviceProvider.GetRequiredService<IConnectionGetter>();
            return Task.FromResult(connectionGetter.GetMarkets(request.Markets));
        }
    }
}