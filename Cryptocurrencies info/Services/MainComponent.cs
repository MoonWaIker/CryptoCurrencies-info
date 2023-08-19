using Cryptocurrencies_info.Services.Interfaces.Main;

namespace Cryptocurrencies_info.Services
{
    public class MainComponent : IMainInterface
    {
        protected IHandler? mediator;

        void IMainInterface.SetMediator(IHandler mediator)
        {
            this.mediator = mediator;
        }
    }
}