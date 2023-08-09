using Newtonsoft.Json;

namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class Market
    {
        [JsonProperty("exchangeId")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("priceUsd")]
        public decimal Price { get; set; }
        public string? Logo { get; set; } = string.Empty;
        [JsonProperty("baseSymbol")]
        public string Base { get; set; } = string.Empty;
        [JsonProperty("quoteSymbol")]
        public string Target { get; set; } = string.Empty;
        public string Trust { get; set; } = string.Empty;
        public string? Link { get; set; } = string.Empty;
    }
}