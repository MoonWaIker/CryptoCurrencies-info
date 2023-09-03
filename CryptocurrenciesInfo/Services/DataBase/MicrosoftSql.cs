using Cryptocurrencies_info.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrencies_info.Services.DataBase
{
    public class MicrosoftSql : EntityFramework
    {

        // Hardcodes
        private readonly string connectionString;

        public MicrosoftSql(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Set configurations
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

            // Connection string
            string? database = configuration.GetValue<string>("database");
            bool trustedConnection = configuration.GetValue<bool>("trustedConnection");
            connectionString = @$"Server=(localdb)\mssqllocaldb;Database={database};Trusted_Connection={trustedConnection};";

            // Markets setting
            Markets = Set<CoinGeckoMarket>();
        }

        // Configuration of connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.EnableSensitiveDataLogging();
            _ = optionsBuilder.UseSqlServer(connectionString);
        }
    }
}