using GameZoneV1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GameZoneV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGamesService _gamesService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , IGamesService gamesService)
        {
            _logger = logger;
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}