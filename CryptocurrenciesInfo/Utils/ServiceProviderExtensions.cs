using CryptocurrenciesInfo.Services;
using CryptocurrenciesInfo.Services.CryptoCurrencies;
using CryptocurrenciesInfo.Services.Interfaces;
using CryptocurrenciesInfo.Services.Interfaces.CoinMarket;
using CryptocurrenciesInfo.Services.Interfaces.Main;

namespace CryptocurrenciesInfo.Utils;

internal static class ServiceProviderExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICoinMarket), typeof(CoinMarket));
        services.AddScoped(typeof(IBusinessLogic), typeof(BusinessLogic));
        // FIXME: This is a bug, the IHandler should be registered as a mediator handler
        services.AddScoped(typeof(IHandler), typeof(Handler));
        services.AddHostedService<CoinGecko>();
    }
}