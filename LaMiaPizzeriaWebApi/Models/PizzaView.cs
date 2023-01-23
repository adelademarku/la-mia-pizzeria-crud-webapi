using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaMiaPizzeriaWebApi.Models
{
    public class PizzaView
    {
        public Pizza Pizze { get; set; }

        public List<Category>? Categories { get; set; }

        public List<SelectListItem>? Ingredienti { get; set; }

       
        public List<string>? TagsSelectedFromMultipleSelect { get; set; }
    }
}
