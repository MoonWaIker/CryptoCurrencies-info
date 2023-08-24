using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrencies_info.Models.DataBase
{
    [Table("coinmarket")]
    [PrimaryKey(nameof(Name), nameof(Base), nameof(Target))]
    public class CoinGeckoMarket : MarketBase
    {
        public string? Logo { get; set; } = string.Empty;
        public string Trust { get; set; } = string.Empty;
        public string? Link { get; set; } = string.Empty;
    }
}