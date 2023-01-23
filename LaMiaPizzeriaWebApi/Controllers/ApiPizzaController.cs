using LaMiaPizzeriaWebApi.Database;
using LaMiaPizzeriaWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LaMiaPizzeriaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiPizzaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizza = db.Pizze.ToList<Pizza>();

                return Ok(pizza);
            }
        }

    }
}
