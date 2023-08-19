using System.Data;
using Cryptocurrencies_info.Services.CryptoCurrencies;
using Cryptocurrencies_info.Services.DataBase;
using Cryptocurrencies_info.Services.Interfaces;
using Cryptocurrencies_info.Services.Interfaces.CoinMarket;
using Cryptocurrencies_info.Services.Interfaces.Connection;
using Cryptocurrencies_info.Services.Interfaces.Main;

namespace Cryptocurrencies_info.Services
{
    public static class ServiceProviderExtensions
    {
        // Providers dictionary
        private static readonly Dictionary<string, Type> databaseProviders = new()
        {
            { "postgre", typeof(PostgreSql) },
            { "microsoft", typeof(MicrosoftSql) }
            // Add more entries for other providers as needed
        };

        public static void AddServices(this IServiceCollection services)
        {
            // Building service provider
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Getting config
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            string databaseProvider = (configuration.GetValue<string>("provider") ?? throw new NoNullAllowedException(nameof(databaseProvider)))
            .ToLowerInvariant();

            // Getting the type of the database provider based on the configuration value
            _ = services.AddTransient(typeof(IConnectionGetter), databaseProviders[databaseProvider]);
            _ = services.AddTransient(typeof(IConnectionFiller), databaseProviders[databaseProvider]);

            // Adding other services
            _ = services.AddTransient(typeof(ICoinMarketExtended), typeof(CoinMarket));
            _ = services.AddTransient(typeof(ICoinMarketBase), typeof(CoinMarket));
            _ = services.AddTransient(typeof(IBuisnessLogic), typeof(BuisnessLogic));
            _ = services.AddTransient<CoinGecko>();
            _ = services.AddTransient(typeof(IHandler), typeof(Handler));
        }
    }
}