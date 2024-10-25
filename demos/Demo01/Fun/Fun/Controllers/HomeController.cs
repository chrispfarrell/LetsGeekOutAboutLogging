using Fun.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Fun.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var firstName = "Chris";
            var lastName = "Farrell";

            // Structured Logging differences...

            // Don't do this
            _logger.LogInformation($"Don't log {firstName} or {lastName} like this");

            // Do this
            _logger.LogInformation("Don't log {firstName} or {lastName} like this",firstName,lastName);

            return View();
        }

        public IActionResult Privacy()
        {
            throw new Exception("Oh no");            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
