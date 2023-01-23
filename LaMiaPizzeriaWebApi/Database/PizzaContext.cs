using LaMiaPizzeriaWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LaMiaPizzeriaWebApi.Database
{
    public class PizzaContext: DbContext
    {
        public DbSet<Pizza> Pizze { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Ingrediente> ingredienti { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost; Database=WebAppPizza;" + "Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
