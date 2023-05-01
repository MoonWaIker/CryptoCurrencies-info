public class CoinFull : Coin
{
    public Dictionary<string, decimal> Markets { get; set; }

    public void SetMarkets(CoinMarket coinMarket)
    {
        Markets = coinMarket.GetMarkets(Id);
    }
}
