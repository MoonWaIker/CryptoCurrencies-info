using System.Data;
using Cryptocurrencies_info.Services.CryptoCurrencies;
using Cryptocurrencies_info.Services.DataBase;

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
            _ = services.AddTransient(typeof(IConnection), databaseProviders[databaseProvider]);

            // Adding other services
            _ = services.AddTransient<CoinMarket>();
            _ = services.AddTransient<BusinessLogic>();
            _ = services.AddTransient<CoinGecko>();
            _ = services.AddTransient<Handler>();
        }
    }
}