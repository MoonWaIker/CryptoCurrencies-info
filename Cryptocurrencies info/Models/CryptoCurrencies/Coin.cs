using Newtonsoft.Json;

public class Coin
{
    [JsonProperty("market_cap_rank")]
    public int Rank { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("current_price")]
    public decimal CurrentPrice { get; set; }

    [JsonProperty("price_change_percentage_24h")]
    public decimal PriceChangePercentage24h { get; set; }

    [JsonProperty("total_volume")]
    public decimal Volume { get; set; }
}