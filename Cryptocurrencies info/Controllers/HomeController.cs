using System.Diagnostics;
using Cryptocurrencies_info.Models;
using Cryptocurrencies_info.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocurrencies_info.Controllers
{
    public class HomeController : Controller
    {
        private readonly Handler _handler;

        // Add logger
        public HomeController(Handler handler)
        {
            _handler = handler;
        }

        // Do not forget set atributes
        public IActionResult Index()
        {
            return View(_handler.coinMarket.GetCoinMarket(10));
        }

        public IActionResult List(int pageNumber, string searchString)
        {
            return View(_handler.processing.Pagination(pageNumber, searchString));
        }

        public IActionResult Coin(string id)
        {
            return View((_handler.coinMarket.GetCoin(id), _handler.coinMarket.GetMarkets(id)));
        }

        public IActionResult Calculator()
        {
            return View(_handler.coinMarket.GetCoinArray());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}