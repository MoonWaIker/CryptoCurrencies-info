using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CryptocurrenciesInfo.Models.Cryptocurrencies
{
    public class Coin
    {
        [Range(1, int.MaxValue)]
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [MinLength(1)]
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [MinLength(1)]
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("priceUsd")]
        public decimal CurrentPrice { get; set; }

        [JsonProperty("changePercent24Hr")]
        public decimal PriceChangePercentage24h { get; set; }

        [JsonProperty("volumeUsd24Hr")]
        public decimal Volume { get; set; }
    }
}