using LaMiaPizzeriaWebApi.Database;
using LaMiaPizzeriaWebApi.Models;
using LaMiaPizzeriaWebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LaMiaPizzeriaWebApi.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> listaDellePizze = db.Pizze.ToList<Pizza>();
                return View("Index", listaDellePizze);
            }
        }

        public IActionResult Details(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                // LINQ: syntax methos
                Pizza pizzaTrovata = db.Pizze
                    .Where(SingolaPizzaNelDb => SingolaPizzaNelDb.Id == id)
                    .Include(pizza => pizza.Category)
                    .Include(pizza => pizza.Ingredienti)
                    .FirstOrDefault();

                if (pizzaTrovata != null)
                {
                    return View(pizzaTrovata);
                }

                return NotFound("La pizza con l'id cercato non esiste!");

            }

        }

        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Category> categoriesFromDb = db.Categories.ToList<Category>();

                PizzaView modelForView = new PizzaView();
                modelForView.Pizze = new Pizza();

                modelForView.Categories = categoriesFromDb;
                modelForView.Ingredienti = IngredientiConverter.getListIngredientiForMultipleSelect();

                return View("Create", modelForView);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaView formData)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();
                    formData.Categories = categories;


                    formData.Ingredienti = IngredientiConverter.getListIngredientiForMultipleSelect();
                }


                return View("Create", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                if (formData.TagsSelectedFromMultipleSelect != null)
                {
                    formData.Pizze.Ingredienti = new List<Ingrediente>();

                    foreach (string ingredienteId in formData.TagsSelectedFromMultipleSelect)
                    {
                        int ingredienteIdIntFromSelect = int.Parse(ingredienteId);

                        Ingrediente ingredienti = db.ingredienti.Where(ingredientiDb => ingredientiDb.Id == ingredienteIdIntFromSelect).FirstOrDefault();

                        // todo controllare eventuali altri errori tipo l'id del tag non esiste

                        formData.Pizze.Ingredienti.Add(ingredienti);
                    }
                }

                db.Pizze.Add(formData.Pizze);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Update(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredienti).FirstOrDefault();

                if (pizzaToUpdate == null)
                {
                    return NotFound("La pizza non è stata trovata");
                }

                List<Category> categories = db.Categories.ToList<Category>();

                PizzaView modelForView = new PizzaView();
                modelForView.Pizze = pizzaToUpdate;
                modelForView.Categories = categories;


                List<Ingrediente> listIngredientiFromDb = db.ingredienti.ToList<Ingrediente>();

                List<SelectListItem> listaOpzioniPerLaSelect = new List<SelectListItem>();

                foreach (Ingrediente ingredienti in listIngredientiFromDb)
                {
                    // Ricerco se il tag che sto inserindo nella lista delle opzioni della select era già stato selezionato dall'utente
                    // all'interno della lista dei tag del post da modificare
                    bool eraStatoSelezionato = pizzaToUpdate.Ingredienti.Any(ingredientiSelezionati => ingredientiSelezionati.Id == ingredienti.Id);

                    SelectListItem opzioneSingolaSelect = new SelectListItem() { Text = ingredienti.Title, Value = ingredienti.Id.ToString(), Selected = eraStatoSelezionato };
                    listaOpzioniPerLaSelect.Add(opzioneSingolaSelect);
                }

                modelForView.Ingredienti = listaOpzioniPerLaSelect;

                return View("Update", modelForView);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaView formData)
        {
            if (!ModelState.IsValid)
            {
                
                using (PizzaContext db = new PizzaContext())
                {
                    List<Category> categories = db.Categories.ToList<Category>();

                    formData.Categories = categories;
                }

                return View("Update", formData);
            }

            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToUpdate = db.Pizze.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredienti).FirstOrDefault();

                if (pizzaToUpdate != null)
                {

                    pizzaToUpdate.Name = formData.Pizze.Name;
                    pizzaToUpdate.Description = formData.Pizze.Description;
                    pizzaToUpdate.Image = formData.Pizze.Image;
                    pizzaToUpdate.Price= formData.Pizze.Price;
                    pizzaToUpdate.Favorites= formData.Pizze.Favorites;
                    pizzaToUpdate.CategoryId = formData.Pizze.CategoryId;

                    // rimuoviamo gli ingredienti e inseriamo i nuovi
                    pizzaToUpdate.Ingredienti.Clear();

                    if (formData.TagsSelectedFromMultipleSelect != null)
                    {

                        foreach (string ingredientiId in formData.TagsSelectedFromMultipleSelect)
                        {
                            int ingredientiIdIntFromSelect = int.Parse(ingredientiId);

                            Ingrediente ingrediente = db.ingredienti.Where(ingredienteDb => ingredienteDb.Id == ingredientiIdIntFromSelect).FirstOrDefault();

                            // todo controllare eventuali altri errori tipo l'id del tag non esiste

                            pizzaToUpdate.Ingredienti.Add(ingrediente);
                        }
                    }

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza che volevi modificare non è stata trovata!");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizzaToDelete = db.Pizze.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizzaToDelete != null)
                {
                    db.Pizze.Remove(pizzaToDelete);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("La pizza da eliminare non è stata trovata!");
                }
            }
        }



    }
}
