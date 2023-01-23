using LaMiaPizzeriaWebApi.Database;
using LaMiaPizzeriaWebApi.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaMiaPizzeriaWebApi.Utils
{
    public static class IngredientiConverter
    {
        public static List<SelectListItem> getListIngredientiForMultipleSelect()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Ingrediente> ingredientiFromDb = db.ingredienti.ToList<Ingrediente>();

                // Creare una lista di SelectListItem e tradurci al suo interno tutti i nostri Ingredienti che provengono da Db
                List<SelectListItem> listaPerLaSelectMultipla = new List<SelectListItem>();

                foreach (Ingrediente ingredienti in ingredientiFromDb)
                {
                    SelectListItem opzioneSingolaSelectMultipla = new SelectListItem() { Text = ingredienti.Title, Value = ingredienti.Id.ToString() };
                    listaPerLaSelectMultipla.Add(opzioneSingolaSelectMultipla);
                }

                return listaPerLaSelectMultipla;
            }
        }

    }
}
