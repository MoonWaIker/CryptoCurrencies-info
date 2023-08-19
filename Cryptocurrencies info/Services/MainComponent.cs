using Cryptocurrencies_info.Services.Interfaces.Main;

namespace Cryptocurrencies_info.Services
{
    public class MainComponent : IMainInterface
    {
        public required IHandler Mediator { get; set; }
    }
}