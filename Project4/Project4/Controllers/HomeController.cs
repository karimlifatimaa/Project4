using Business.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Project4.Models;
using System.Diagnostics;

namespace Project4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeamServices _teamServices;
        public HomeController(ILogger<HomeController> logger, ITeamServices teamServices)
        {
            _logger = logger;
            _teamServices = teamServices;
        }

        public IActionResult Index()
        {
            var teams=_teamServices.GetAllTeams();
            return View(teams);
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
