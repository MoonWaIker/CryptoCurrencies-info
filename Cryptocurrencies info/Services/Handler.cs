using Cryptocurrencies_info.Services.Interfaces;
using Cryptocurrencies_info.Services.Interfaces.CoinMarket;
using Cryptocurrencies_info.Services.Interfaces.Connection;

namespace Cryptocurrencies_info.Services
{
    public sealed class Handler
    {
        public readonly ICoinMarketExtended coinMarketExtanded;
        public readonly ICoinMarketBase coinMarketBase;
        public readonly IBuisnessLogic buisnessLogic;
        public readonly IConnectionGetter connectionGetter;
        public readonly IConnectionFiller connectionFiller;

        public Handler(IServiceProvider serviceProvider)
        {
            coinMarketExtanded = serviceProvider.GetRequiredService<ICoinMarketExtended>();
            coinMarketBase = serviceProvider.GetRequiredService<ICoinMarketBase>();
            buisnessLogic = serviceProvider.GetRequiredService<IBuisnessLogic>();
            connectionGetter = serviceProvider.GetRequiredService<IConnectionGetter>();
            connectionFiller = serviceProvider.GetRequiredService<IConnectionFiller>();
        }
    }
}