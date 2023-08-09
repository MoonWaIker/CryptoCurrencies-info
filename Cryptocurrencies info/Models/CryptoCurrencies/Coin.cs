using Newtonsoft.Json;

namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class Coin
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
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