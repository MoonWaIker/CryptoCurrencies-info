public static class ServiceProviderExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<CoinMarketDB>();
        services.AddTransient<CoinMarket>();
        services.AddTransient<Processing>();
        services.AddTransient<CoinGecko>();
        services.AddTransient<Handler>();
    }
}