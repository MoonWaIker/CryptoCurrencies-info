public static class ServiceProviderExtensions
{
    // Providers dictionary
    private static readonly Dictionary<string, Type> databaseProviders = new()
        {
            { "postgre", typeof(PostgreSql) },
            { "microsoft", typeof(MsSql) }
            // Add more entries for other providers as needed
        };

    public static void AddServices(this IServiceCollection services)
    {
        // Building service provider
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        // Getting config
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        string databaseProvider = (configuration.GetValue<string>("provider") ?? throw new ArgumentNullException("Provider must be not null"))
        .ToLowerInvariant();

        // Getting the type of the database provider based on the configuration value
        services.AddTransient(typeof(IConnection), databaseProviders[databaseProvider]);

        // Adding other services
        services.AddTransient<CoinMarket>();
        services.AddTransient<Processing>();
        services.AddTransient<CoinGecko>();
        services.AddTransient<Handler>();
    }
}