using Microsoft.Extensions.Hosting;

namespace LaMiaPizzeriaWebApi.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<Pizza> Pizze { get; set; }

        public Ingrediente() { }
    }
}
