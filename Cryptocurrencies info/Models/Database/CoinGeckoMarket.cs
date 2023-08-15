using System.ComponentModel.DataAnnotations;

namespace Cryptocurrencies_info.Models.DataBase
{
    public class CoinGeckoMarket : MarketBase
    {
        public string? Logo { get; set; } = string.Empty;
        public string Trust { get; set; } = string.Empty;
        public string? Link { get; set; } = string.Empty;
    }
}