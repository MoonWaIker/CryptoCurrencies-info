using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CryptocurrenciesInfo.Models.Cryptocurrencies
{
    public class Coin
    {
        [Range(1, int.MaxValue)]
        [JsonProperty("rank", NullValueHandling = NullValueHandling.Ignore)]
        public int Rank { get; set; }

        [MinLength(1)] [JsonProperty("name")] public string Name { get; set; } = string.Empty;

        [MinLength(1)] [JsonProperty("id")] public string Id { get; set; } = string.Empty;

        [JsonProperty("priceUsd", NullValueHandling = NullValueHandling.Ignore)]
        public decimal CurrentPrice { get; set; }

        [JsonProperty("changePercent24Hr", NullValueHandling = NullValueHandling.Ignore)]
        public decimal PriceChangePercentage24H { get; set; }

        [JsonProperty("volumeUsd24Hr", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Volume { get; set; }
    }
}