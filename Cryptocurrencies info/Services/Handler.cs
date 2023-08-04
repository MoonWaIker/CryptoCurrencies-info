public class Handler
{
    public readonly CoinMarket coinMarket;
    public readonly Processing processing;
    public readonly IConnection connection;
    public readonly CoinGecko coinGecko;

    public Handler(CoinMarket coinMarket, Processing processing, IConnection connection, CoinGecko coinGecko)
    {
        this.coinMarket = coinMarket;
        this.processing = processing;
        this.connection = connection;
        this.coinGecko = coinGecko;
    }
}