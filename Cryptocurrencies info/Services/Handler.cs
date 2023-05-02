public class Handler
{
    public readonly CoinMarket coinMarket;
    public readonly Processing processing;

    public Handler(CoinMarket coinMarket, Processing processing)
    {
        this.coinMarket = coinMarket;
        this.processing = processing;
    }
}