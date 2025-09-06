using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.WebAPI.Data
{
    public class CaloriesContext : IdentityDbContext<HealthyUser>
    {
        public CaloriesContext(DbContextOptions<CaloriesContext> options) : base(options) { }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishProduct>  DishProducts { get; set; }

        public DbSet<EatingItem>  EatingItems { get; set; }

        public DbSet<Product>  Products { get; set; }



    }
}
