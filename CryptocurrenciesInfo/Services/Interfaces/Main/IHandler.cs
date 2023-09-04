using CryptocurrenciesInfo.Models.Cryptocurrencies;
using CryptocurrenciesInfo.Models.DataBase;
using CryptocurrenciesInfo.Services.Requests;
using MediatR;

namespace CryptocurrenciesInfo.Services.Interfaces.Main
{
    public interface IHandler : IRequestHandler<PaginationRequest, PaginatedMarkets>, IRequestHandler<CoinMarketRequest,
    IEnumerable<Coin>>, IRequestHandler<CoinArrayRequest, string[]>, IRequestHandler<CoinRequest, CoinFull>, IRequestHandler<DBPutRequest>,
    IRequestHandler<RequestFromDB, IEnumerable<CoinGeckoMarket>>
    { }
}