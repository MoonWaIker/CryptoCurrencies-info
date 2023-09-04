using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces.Connection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cryptocurrencies_info.Services.DataBase
{
    public class EntityFramework : DbContext, IConnectionGetter, IConnectionFiller
    {
        // TODO Is must to have Parameterized Queries here?
        // DataBase
        public DbSet<CoinGeckoMarket> Markets { get; set; }

        private readonly ILogger<EntityFramework> logger;

        public EntityFramework(IServiceProvider serviceProvider)
        {
            logger = serviceProvider.GetRequiredService<ILogger<EntityFramework>>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.EnableSensitiveDataLogging();
        }

        // Add markets
        public void AddMarkets(IEnumerable<CoinGeckoMarket> markets)
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
