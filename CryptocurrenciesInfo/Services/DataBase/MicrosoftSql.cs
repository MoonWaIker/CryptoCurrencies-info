using CryptocurrenciesInfo.Models.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CryptocurrenciesInfo.Services.DataBase
{
    public class MicrosoftSql : EntityFramework
    {

        private readonly string connectionString;

        public MicrosoftSql(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Set configurations
            IConfigurationSection database = serviceProvider.GetRequiredService<IConfiguration>().GetSection("DataBase");

            string host = database.GetValue<string>("host") ?? throw new ArgumentNullException(nameof(serviceProvider), "Host must be not null");
            string databaseName = database.GetValue<string>("database") ?? string.Empty;
            string user = database.GetValue<string>("username") ?? string.Empty;
            string password = database.GetValue<string>("password") ?? string.Empty;
            bool trustedConnection = database.GetValue<bool>("trustedConnection");
            bool trustServerCertificate = database.GetValue<bool>("trustServerCertificate");

            connectionString = @$"Server={host};{(databaseName.IsNullOrEmpty() ? string.Empty : $"Database ={databaseName}")};Trusted_Connection={trustedConnection};Trust Server Certificate={trustServerCertificate};{(user.IsNullOrEmpty() ? string.Empty : $"User ID={user};")}{(password.IsNullOrEmpty() ? string.Empty : $"Password={password};")}";

            Markets = Set<CoinGeckoMarket>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.UseSqlServer(connectionString);
        }
    }
}