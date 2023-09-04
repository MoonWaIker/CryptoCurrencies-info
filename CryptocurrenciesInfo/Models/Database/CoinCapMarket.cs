using Newtonsoft.Json;

namespace CryptocurrenciesInfo.Models.DataBase
{
    public class CoinCapMarket : MarketBase
    {
        [JsonProperty("exchangeId")]
        public override string Name { get; set; } = string.Empty;

        [JsonProperty("priceUsd")]
        public decimal Price { get; set; }

        [JsonProperty("baseSymbol")]
        public override string Base { get; set; } = string.Empty;

        [JsonProperty("quoteSymbol")]
        public override string Target { get; set; } = string.Empty;
    }
}