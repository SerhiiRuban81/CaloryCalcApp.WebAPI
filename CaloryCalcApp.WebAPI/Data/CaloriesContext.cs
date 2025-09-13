using CaloryCalcApp.WebAPI.Models.DTOs.Dish;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProduct;
using CaloryCalcApp.WebAPI.Models.DTOs.Product;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CaloryCalcApp.WebAPI.Data
{
    public class CaloriesContext : IdentityDbContext<HealthyUser>
    {
        public CaloriesContext(DbContextOptions<CaloriesContext> options) : base(options) { }

        public DbSet<DishDTO> Dishes { get; set; }

        public DbSet<DishProductDTO>  DishProducts { get; set; }

        public DbSet<HealthyUserDishDTO> HealthyUserDishes { get; set; }

        public DbSet<ProductDTO>  Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Dish>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Dishes)
                .UsingEntity<DishProduct>();

            builder.Entity<HealthyUser>()
                .HasMany(e => e.Dishes)
                .WithMany(e => e.HealthyUsers)
                .UsingEntity<HealthyUserDishDTO>(e =>
                e.Property(c => c.MealTime).HasDefaultValueSql("GETUTCDATE()"));
        }

    }
}
