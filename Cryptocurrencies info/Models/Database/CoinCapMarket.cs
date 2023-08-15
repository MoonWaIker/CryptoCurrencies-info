using Newtonsoft.Json;

namespace Cryptocurrencies_info.Models.DataBase
{
    public class CoinCapMarket : MarketBase
    {
        [JsonProperty("exchangeId")]
        public new string Name { get; set; } = string.Empty;
        [JsonProperty("priceUsd")]
        public decimal Price { get; set; }
        [JsonProperty("baseSymbol")]
        public new string Base { get; set; } = string.Empty;
        [JsonProperty("quoteSymbol")]
        public new string Target { get; set; } = string.Empty;
    }
}