using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cryptocurrencies_info.Models.DataBase
{
    [Table("coinmarket")]
    [PrimaryKey(nameof(Name), nameof(Base), nameof(Target))]
    public class CoinGeckoMarket : MarketBase
    {
        [JsonProperty("base")]
        public override string Base { get; set; } = string.Empty;
        [JsonProperty("target")]
        public override string Target { get; set; } = string.Empty;
        public string? Logo { get; set; } = string.Empty;
        [JsonProperty("trust_score")]
        public string Trust { get; set; } = string.Empty;
        [JsonProperty("trade_url")]
        public string? Link { get; set; } = string.Empty;

        [JsonExtensionData]
        protected IDictionary<string, JToken>? additionalData;

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext context)
        {
            Name = (string)additionalData!["market"]["name"]!;
            Logo = (string)additionalData["market"]["logo"]!;
        }
    }
}