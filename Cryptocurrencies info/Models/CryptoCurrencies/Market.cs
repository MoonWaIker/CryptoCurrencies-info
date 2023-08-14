using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class Market
    {
        [MinLength(1)]
        [JsonProperty("exchangeId")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("priceUsd")]
        public decimal Price { get; set; }
        public string? Logo { get; set; } = string.Empty;
        [MinLength(1)]
        [JsonProperty("baseSymbol")]
        public string Base { get; set; } = string.Empty;
        [MinLength(1)]
        [JsonProperty("quoteSymbol")]
        public string Target { get; set; } = string.Empty;
        public string Trust { get; set; } = string.Empty;
        public string? Link { get; set; } = string.Empty;
    }
}