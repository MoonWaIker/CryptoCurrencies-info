using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Models.DataBase;
using CryptocurrenciesInfo.Services.Interfaces;
using CryptocurrenciesInfo.Services.Interfaces.CoinMarket;
using CryptocurrenciesInfo.Services.Interfaces.Connection;
using CryptocurrenciesInfo.Services.Interfaces.Main;
using CryptocurrenciesInfo.Services.Requests;
using MediatR;

namespace CryptocurrenciesInfo.Services
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
        public async Task<PaginatedMarkets> Handle(PaginationRequest request, CancellationToken cancellationToken)
        {
            return await buisnessLogic.Pagination(request.PageNumber, request.SearchString, cancellationToken);
        }

        Task<IEnumerable<Coin>> IRequestHandler<CoinMarketRequest, IEnumerable<Coin>>.Handle(CoinMarketRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() => request.Limit == 0 ? coinMarketBase.GetCoinMarket() : coinMarketExtanded.GetCoinMarket(request.Limit));
        }

        public Task<string[]> Handle(CoinArrayRequest request, CancellationToken cancellationToken)
        {
            return Task.Run(() => coinMarketExtanded.GetCoinArray());
        }

        public async Task<CoinFull> Handle(CoinRequest request, CancellationToken cancellationToken)
        {
            return await coinMarketExtanded.GetCoin(request.Id, cancellationToken);
        }

        public async Task Handle(DBPutRequest request, CancellationToken cancellationToken)
        {
            IConnectionFiller connectionFiller = serviceProvider.GetRequiredService<IConnectionFiller>();
            await Task.Run(() => connectionFiller.AddMarkets(request.CoinGeckoMarkets), cancellationToken);
        }

        public Task<IEnumerable<CoinGeckoMarket>> Handle(RequestFromDB request, CancellationToken cancellationToken)
        {
            IConnectionGetter connectionGetter = serviceProvider.GetRequiredService<IConnectionGetter>();
            return Task.Run(() => connectionGetter.GetMarkets(request.Markets));
        }
    }
}