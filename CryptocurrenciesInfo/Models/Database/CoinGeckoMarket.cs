using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using CryptocurrenciesInfo.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CryptocurrenciesInfo.Models.DataBase
{
    [Table("CoinMarket")]
    [PrimaryKey(nameof(Id))]
    public class CoinGeckoMarket : MarketBase
    {
        private const int MaxUrlLength = 2048;

        [JsonExtensionData] private IDictionary<string, JToken> additionalData;
        public int Id { get; init; }

        [JsonProperty("base")] public override required string Base { get; set; }

        [JsonProperty("target")] public override required string Target { get; set; }

        [MaxLength(MaxUrlLength)] public string Logo { get; set; } = string.Empty;

        [JsonProperty("trust_score", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Trust Trust { get; set; }

        [MaxLength(MaxUrlLength)]
        [JsonProperty("trade_url")]
        // TODO Check for null inserting
        public string Link { get; set; } = string.Empty;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            var market = additionalData["market"];

            Name = (string?)market["name"] ?? throw new JsonSerializationException();
            Logo = (string?)market["logo"] ?? string.Empty;
        }
    }
}