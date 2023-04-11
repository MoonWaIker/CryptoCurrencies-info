using Newtonsoft.Json;

public class Coin
{
    [JsonProperty("rank")]
    public int Rank { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("id")]
    public string Id { get; set; } = "";

    [JsonProperty("priceUsd")]
    public decimal CurrentPrice { get; set; }

    [JsonProperty("changePercent24Hr")]
    public decimal PriceChangePercentage24h { get; set; }

    [JsonProperty("volumeUsd24Hr")]
    public decimal Volume { get; set; }
}