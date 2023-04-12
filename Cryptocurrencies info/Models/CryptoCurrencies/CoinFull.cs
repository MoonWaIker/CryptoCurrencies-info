using System.Runtime.Serialization;

public class CoinFull : Coin
{
    public Dictionary<string, decimal> Markets {  get; set; }

    [OnDeserialized]
    protected void OnDeserialized(StreamingContext context) 
    {
        Markets = new CoinMarket().GetMarkets(Id);
    }
}
