using Kenpo.Configurations;
using Kenpo.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Kenpo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<AppSettings> _config;

        public HomeController(IOptions<AppSettings> config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel(_config);
            return View(model);
        }

        public IActionResult About()
        {
            var model = new AboutViewModel(_config);
            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
