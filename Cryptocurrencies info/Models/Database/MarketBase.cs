using System.ComponentModel.DataAnnotations;

namespace Cryptocurrencies_info.Models.DataBase
{
    public class MarketBase
    {
        [MinLength(1)]
        public string Name { get; set; } = string.Empty;
        [MinLength(1)]
        public string Base { get; set; } = string.Empty;
        [MinLength(1)]
        public string Target { get; set; } = string.Empty;
    }
}