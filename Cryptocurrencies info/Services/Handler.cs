public class Handler
{
    public readonly CoinMarket coinMarket;
    public readonly Processing processing;
    public readonly CoinMarketDB coinMarketDB;
    public readonly CoinGecko coinGecko;

    public Handler(CoinMarket coinMarket, Processing processing, CoinMarketDB coinMarketDB, CoinGecko coinGecko)
    {
        this.coinMarket = coinMarket;
        this.processing = processing;
        this.coinMarketDB = coinMarketDB;
        this.coinGecko = coinGecko;
    }
}