using LaMiaPizzeriaWebApi.Database;
using LaMiaPizzeriaWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LaMiaPizzeriaWebApi.Controllers
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

            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> listaDellePizze = db.Pizze.ToList<Pizza>();
                return View( listaDellePizze);
            }

        }

        public IActionResult Lista()
        {

            return View();

        }

        public IActionResult Details()
        {
            return View();
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