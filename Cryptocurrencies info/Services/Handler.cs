using Cryptocurrencies_info.Services.CryptoCurrencies;
using Cryptocurrencies_info.Services.DataBase;

namespace Cryptocurrencies_info.Services
{
    public sealed class Handler
    {
        public readonly CoinMarket coinMarket;
        public readonly BusinessLogic processing;
        public readonly IConnection connection;
        public readonly CoinGecko coinGecko;

        // TODO Get services by service provider
        public Handler(CoinMarket coinMarket, BusinessLogic processing, IConnection connection, CoinGecko coinGecko)
        {
            this.coinMarket = coinMarket;
            this.processing = processing;
            this.connection = connection;
            this.coinGecko = coinGecko;
        }
    }
}