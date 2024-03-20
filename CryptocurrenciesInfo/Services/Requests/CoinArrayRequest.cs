using MediatR;

namespace CryptocurrenciesInfo.Services.Requests;

public sealed class CoinArrayRequest : IRequest<IEnumerable<string>>;