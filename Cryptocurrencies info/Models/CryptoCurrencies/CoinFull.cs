using Newtonsoft.Json;
using System.Runtime.Serialization;

public class CoinFull : Coin
{
    private readonly CoinMarket _coinMarket;
    public Dictionary<string, decimal> Markets {  get; set; }

    // Fix this
    [JsonConstructor]
    public CoinFull(CoinMarket coinMarket) 
    {
        _coinMarket = new CoinMarket();
    }

    [OnDeserialized]
    protected void OnDeserialized(StreamingContext context) 
    {
        Markets = _coinMarket.GetMarkets(Id);
    }
}
