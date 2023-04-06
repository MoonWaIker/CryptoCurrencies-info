using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

class CoinFull : Coin
{
    public string[] Markets { get; set; }

    [JsonExtensionData]
    protected IDictionary<string, JToken> additionalData;

    [OnDeserialized]
    protected void OnDeserialized(StreamingContext context)
    {
        Markets = additionalData["tickers"].Select(i => (string)i["market"]["name"]).Distinct().ToArray();
        PriceChangePercentage24h = (decimal)additionalData["market_data"]["price_change_percentage_24h"];
        CurrentPrice = (decimal)additionalData["market_data"]["current_price"]["usd"];
        Volume = (decimal)additionalData["market_data"]["total_volume"]["usd"];
    }
}
