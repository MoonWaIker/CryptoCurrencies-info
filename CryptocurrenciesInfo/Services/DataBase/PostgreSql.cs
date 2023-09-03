using Cryptocurrencies_info.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrencies_info.Services.DataBase
{
    // TODO Some exception at the start
    public class PostgreSql : EntityFramework
    {
        // Hardcodes
        private readonly string connectionString;

        public PostgreSql(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Set configurations
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Connection string
            string host = configuration.GetValue<string>("host") ?? throw new ArgumentNullException(nameof(serviceProvider), "Host must be not null");
            int port = configuration.GetValue<int>("port");
            string? username = configuration.GetValue<string>("username");
            string? database = configuration.GetValue<string>("database");
            bool trustedConnection = configuration.GetValue<bool>("trustedConnection");
            bool errorDetail = configuration.GetValue<bool>("errorDetail");
            connectionString = $"Host={host};Port={port};" +
            $"{(username is not null ? $"Username={username};" : string.Empty)}" +
            $"{(database is not null ? $"Database={database};" : string.Empty)}" +
            $"Trust Server Certificate={trustedConnection};" +
            $"Include Error Detail={errorDetail};";

            // Markets setting
            Markets = Set<CoinGeckoMarket>();
        }

        // Configuration of connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.EnableSensitiveDataLogging();
            _ = optionsBuilder.UseNpgsql(connectionString);
        }
    }
}