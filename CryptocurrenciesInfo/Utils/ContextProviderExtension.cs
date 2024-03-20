using System.Data;
using CryptocurrenciesInfo.DataBase;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrenciesInfo.Utils;

internal static class ContextProviderExtension
{
    private const string ConnectionStringOption = "Connection";
    private const string ProviderOption = "Provider";

    private static string _connectionString = string.Empty;

    private static readonly Dictionary<string, Action<DbContextOptionsBuilder>> UseSqlDelegates = new()
    {
        { "microsoft", static options => options.UseSqlServer(_connectionString) },
        { "postgres", static options => options.UseNpgsql(_connectionString) }
    };

    public static void AddContext(this IServiceCollection services, IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringOption)
                            ?? throw new NoNullAllowedException(ExceptionMessages.ConnectionStringIsNullError);

        var provider = UseSqlDelegates[configuration.GetConnectionString(ProviderOption)
                                       ?? throw new NoNullAllowedException(ExceptionMessages.ProviderIsNullError)];

        services.AddDbContext<MarketsContext>(provider, ServiceLifetime.Transient, ServiceLifetime.Transient);
        services.AddDbContextFactory<MarketsContext>(provider);
    }
}