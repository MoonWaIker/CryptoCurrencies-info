public class Processing
{
    private const int size = 100;
    private readonly CoinMarket _coinMarket;

    public Processing(CoinMarket coinMarket)
    {
        _coinMarket = coinMarket;
    }

    public Object Pagination(int pageNumber, string searchString)
    {
        var coins = _coinMarket.GetCoinMarket();
        if (!string.IsNullOrEmpty(searchString))
            coins = coins
                .Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString))
                .ToArray();
        return new
        {
            Data = coins
            .Skip(size * pageNumber)
            .Take(size),
            PageNumber = pageNumber,
            MaxPages = coins.Count() / size - 1,
            Size = size
        };
    }
}
