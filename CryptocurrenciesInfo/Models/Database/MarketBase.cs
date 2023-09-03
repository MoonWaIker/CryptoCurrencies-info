using System.ComponentModel.DataAnnotations;

namespace Cryptocurrencies_info.Models.DataBase
{
    public class MarketBase
    {
        [MinLength(1)]
        public virtual string Name { get; set; } = string.Empty;

        [MinLength(1)]
        public virtual string Base { get; set; } = string.Empty;

        [MinLength(1)]
        public virtual string Target { get; set; } = string.Empty;
    }
}