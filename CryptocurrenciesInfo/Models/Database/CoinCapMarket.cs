using Newtonsoft.Json;

namespace CryptocurrenciesInfo.Models.DataBase
{
    public sealed class CoinCapMarket : MarketBase
    {
        [JsonProperty("exchangeId")] public override required string Name { get; set; }

        [JsonProperty("priceUsd")] public required decimal Price { get; set; }

        [JsonProperty("baseSymbol")] public override required string Base { get; set; }

        [JsonProperty("quoteSymbol")] public override required string Target { get; set; }
    }
}