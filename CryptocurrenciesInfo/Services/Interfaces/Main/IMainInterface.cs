namespace Cryptocurrencies_info.Services.Interfaces.Main
{
    public interface IMainInterface
    {
        public IHandler Mediator { protected get; set; }
    }
}