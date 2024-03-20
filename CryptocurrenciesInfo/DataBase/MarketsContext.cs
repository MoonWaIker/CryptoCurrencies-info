using CryptocurrenciesInfo.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace CryptocurrenciesInfo.DataBase;

public sealed class MarketsContext : DbContext
{
    public MarketsContext(DbContextOptions<MarketsContext> options)
        : base(options)
    {
        Markets = Set<CoinGeckoMarket>();
    }

    public DbSet<CoinGeckoMarket> Markets { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
    }
}