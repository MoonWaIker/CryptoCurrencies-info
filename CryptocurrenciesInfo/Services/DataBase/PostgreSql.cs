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
            var database = configuration.GetSection("DataBase");

            // Connection string
            string host = database.GetValue<string>("host") ?? throw new ArgumentNullException(nameof(serviceProvider), "Host must be not null");
            int port = database.GetValue<int>("port");
            string? username = database.GetValue<string>("username");
            string? databaseName = database.GetValue<string>("database");
            bool trustedConnection = database.GetValue<bool>("trustedConnection");
            bool errorDetail = database.GetValue<bool>("errorDetail");
            connectionString = $"Host={host};Port={port};" +
            $"{(username is not null ? $"Username={username};" : string.Empty)}" +
            $"{(databaseName is not null ? $"Database={databaseName};" : string.Empty)}" +
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