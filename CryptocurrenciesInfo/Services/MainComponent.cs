using CryptocurrenciesInfo.Services.Interfaces.Main;

namespace CryptocurrenciesInfo.Services
{
    public class MainComponent : IMainInterface
    {
        public required IHandler Mediator { get; set; }
    }
}