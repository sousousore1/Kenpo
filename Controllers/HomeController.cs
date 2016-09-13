using Kenpo.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Kenpo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new IndexViewModel();
            return View(model);
        }

        public IActionResult About()
        {
            return View();
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
