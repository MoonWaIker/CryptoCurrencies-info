using CryptocurrenciesInfo.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrenciesInfo.Services.DataBase
{
    public class MicrosoftSql : EntityFramework
    {

        private readonly string connectionString;

        public MicrosoftSql(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Set configurations
            IConfigurationSection database = serviceProvider.GetRequiredService<IConfiguration>().GetSection("DataBase");

            // Connection string
            string? databaseName = database.GetValue<string>("database");
            bool trustedConnection = database.GetValue<bool>("trustedConnection");
            connectionString = @$"Server=(localdb)\mssqllocaldb;Database={databaseName};Trusted_Connection={trustedConnection};";

            // Markets setting
            Markets = Set<CoinGeckoMarket>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(connectionString);
        }
    }
}