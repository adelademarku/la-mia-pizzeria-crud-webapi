using LaMiaPizzeriaWebApi.Areas.Identity.Data;
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
        public IActionResult Get(string? search)
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> prodotto = new List<Pizza>();

                if (search is null || search == "")
                {
                    prodotto = db.Pizze.ToList<Pizza>();
                }
                 else
                 {
                // converto tutto in stringa minuscola, non mi interessano le lettere maiuscole
                  search = search.ToLower();

                  prodotto = db.Pizze.Where(prodotto => prodotto.Name.ToLower().Contains(search))

                                     .ToList<Pizza>();
                 }

                return Ok(prodotto);
             }
        }

            [HttpGet("{id}")]
            public IActionResult Get(int id)
            {

                using (PizzaContext db = new PizzaContext())
                {
                    Pizza prodotto = db.Pizze.Where(prodotto => prodotto.Id == id).FirstOrDefault();

                    if (prodotto is null)
                    {
                        return NotFound("La pizza con questo id non è stato trovato!");
                    }

                    return Ok(prodotto);
                }
            }
    }

    
}
