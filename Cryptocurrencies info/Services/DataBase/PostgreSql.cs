using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cryptocurrencies_info.Services.DataBase
{
    // TODO Is must to have Parameterized Queries here?
    // TODO Some exception at the start
    public class PostgreSql : DbContext, IConnectionGetter, IConnectionFiller
    {
        // DataBase
        public DbSet<CoinGeckoMarket> Markets { get; set; }

        // Hardcodes
        private readonly string connectionString;
        private readonly ILogger<PostgreSql> logger;

        public PostgreSql(IServiceProvider serviceProvider)
        {
            // Initialize logger
            logger = serviceProvider.GetRequiredService<ILogger<PostgreSql>>();

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

        // Add markets
        public void AddMarkets(CoinGeckoMarket[] markets)
        {
            // Transaction
            using IDbContextTransaction transaction = Database.BeginTransaction();

            // Base
            markets = markets
            .DistinctBy(market => new
            {
                market.Name,
                market.Base,
                market.Target
            })
            .ToArray();

            // Add
            IEnumerable<CoinGeckoMarket> marketsToAdd = markets
            .ExceptBy(Markets
            .Select(market => new
            {
                market.Name,
                market.Base,
                market.Target
            }), market => new
            {
                market.Name,
                market.Base,
                market.Target
            });

            // Update
            IEnumerable<CoinGeckoMarket> marketsToUpdate = markets
            .IntersectBy(Markets
            .Select(market => new
            {
                market.Name,
                market.Base,
                market.Target
            }), market => new
            {
                market.Name,
                market.Base,
                market.Target
            });

            try
            {
                // Add and update
                Markets.AddRange(marketsToAdd);
                Markets.UpdateRange(marketsToUpdate);
                _ = SaveChanges();
                transaction.Commit();
            }
            // Rollback
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                try
                {
                    transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    logger.LogError(exRollback.Message, exRollback);
                }
            }
        }

        // Truncate table
        public void RefreshTable()
        {
            using IDbContextTransaction transaction = Database.BeginTransaction();
            try
            {
                Markets.RemoveRange(Markets);
                _ = SaveChanges();
                transaction.Commit();
            }
            // Rollback
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                try
                {
                    transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    logger.LogError(exRollback.Message, exRollback);
                }
            }
        }

        // Get markets
        public IEnumerable<CoinGeckoMarket> GetMarkets(IEnumerable<MarketBase> markets)
        {
            string[] names = markets
                .DistinctBy(market => market.Name)
                .Select(market => market.Name)
                .ToArray();

            var identifires = markets
                .DistinctBy(market => new
                {
                    market.Base,
                    market.Target
                })
                .Select(market => new
                {
                    market.Base,
                    market.Target
                })
                .ToArray();

            return Markets
                .AsEnumerable()
                .Where(market => names.Contains(market.Name) &&
                Array.Exists(identifires, id => market.Base == id.Base && market.Target == id.Target));
        }
    }
}