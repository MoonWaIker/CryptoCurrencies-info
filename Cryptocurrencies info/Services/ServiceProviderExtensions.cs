public static class ServiceProviderExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<CoinMarket>();
        services.AddTransient<Processing>();
    }
}