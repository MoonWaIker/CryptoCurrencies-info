using Cryptocurrencies_info.Services.Interfaces;

namespace Cryptocurrencies_info.Services
{
    public sealed class Handler
    {
        public readonly ICoinMarket coinMarket;
        public readonly IBuisnessLogic buisnessLogic;
        public readonly IConnection connection;

        public Handler(IServiceProvider serviceProvider)
        {
            coinMarket = serviceProvider.GetRequiredService<ICoinMarket>();
            buisnessLogic = serviceProvider.GetRequiredService<IBuisnessLogic>();
            connection = serviceProvider.GetRequiredService<IConnection>();
        }
    }
}