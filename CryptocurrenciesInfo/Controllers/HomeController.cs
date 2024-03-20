using System.Diagnostics;
using CryptocurrenciesInfo.Models;
using CryptocurrenciesInfo.Services.Interfaces.Main;
using CryptocurrenciesInfo.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CryptocurrenciesInfo.Controllers;

// TODO Use the IMediator interface instead of IHandler
// TODO review web sockets
// TODO make normal exception handler (400 for bad requests and 500 for internal errors)
// TODO make exception catching it via Middleware
[Controller]
[Route("[controller]")]
public sealed class HomeController(IHandler mediator) : Controller
{
    private const string ActionRoute = "[action]";
    private const string IndexRoute = "";
    private const string ListRoute = ActionRoute;
    private const string CoinRoute = "[action]/{id?}";
    private const string CalculatorRoute = ActionRoute;
    private const string PrivacyRoute = ActionRoute;
    private const string ErrorRoute = ActionRoute;

    [Route(IndexRoute)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await mediator.Handle(new CoinMarketRequest
        {
            Limit = 10
        }, cancellationToken));
    }

    [Route(ListRoute)]
    public async Task<IActionResult> List(int pageNumber, string searchString, CancellationToken cancellationToken)
    {
        return View(await mediator.Handle(new PaginationRequest
        {
            PageNumber = pageNumber,
            SearchString = searchString
        }, cancellationToken));
    }

    [Route(CoinRoute)]
    public async Task<IActionResult> Coin(string id, CancellationToken cancellationToken)
    {
        return View(await mediator.Handle(new CoinRequest
        {
            Id = id
        }, cancellationToken));
    }

    [Route(CalculatorRoute)]
    public async Task<IActionResult> Calculator(CancellationToken cancellationToken)
    {
        return View(await mediator.Handle(new CoinArrayRequest(), cancellationToken));
    }

    [Route(PrivacyRoute)]
    public IActionResult Privacy()
    {
        return View();
    }

    [Route(ErrorRoute)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}