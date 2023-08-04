using Newtonsoft.Json;

public class Market
{
    [JsonProperty("exchangeId")]
    public string Name { get; set; } = "";

    [JsonProperty("priceUsd")]
    public decimal Price { get; set; }

    public string? Logo { get; set; } = "";

    [JsonProperty("baseSymbol")]
    public string Base { get; set; } = "";

    [JsonProperty("quoteSymbol")]
    public string Target { get; set; } = "";

    public string Trust { get; set; } = "";

    public string? Link { get; set; } = "";
}