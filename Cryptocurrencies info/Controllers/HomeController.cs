using Cryptocurrencies_info.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cryptocurrencies_info.Controllers
{
    public class HomeController : Controller
    {
        private CoinMarket coinMarket = new CoinMarket();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Do not forget set atributes
        public IActionResult Index()
        {
            return View(coinMarket.GetCoinMarket(10));
        }

        public IActionResult List(int pageNumber, string searchString)
        {
            int size = 100;
            var coins = coinMarket.GetCoinMarket();
            // Can I use sugar synt. here?
            if (!string.IsNullOrEmpty(searchString))
                coins = coins.Where(i => i.Name.Contains(searchString) || i.Id.Contains(searchString)).ToArray();
            return View(new
            {
                Data = coins.Skip(size * pageNumber).Take(size),
                PageNumber = pageNumber,
                MaxPages = coins.Count() / size - 1,
                Size = size
            });
        }

        public IActionResult Coin(string id)
        {
            return View(coinMarket.GetCoin(id));
        }

        public IActionResult Calculator()
        {
            return View(coinMarket.GetCoinArray());
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