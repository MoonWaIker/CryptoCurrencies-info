namespace Cryptocurrencies_info.Models.Cryptocurrencies
{
    public class CoinFull : Coin
    {
        public Market[] Markets { get; set; }

        public CoinFull(Coin coin, Market[] markets)
        {
            Rank = coin.Rank;
            Name = coin.Name;
            Id = coin.Id;
            CurrentPrice = coin.CurrentPrice;
            PriceChangePercentage24h = coin.PriceChangePercentage24h;
            Volume = coin.Volume;
            Markets = markets;
        }
    }
}