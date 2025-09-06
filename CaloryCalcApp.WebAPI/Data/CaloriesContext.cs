using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CaloryCalcApp.WebAPI.Data
{
    public class CaloriesContext : IdentityDbContext<HealthyUser>
    {
        public CaloriesContext(DbContextOptions<CaloriesContext> options) : base(options) { }

        public DbSet<Dish> Dishes { get; set; }

        public DbSet<DishProduct>  DishProducts { get; set; }

        public DbSet<EatingItem>  EatingItems { get; set; }

        public DbSet<Product>  Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Dish>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Dishes)
                .UsingEntity<DishProduct>();

            builder.Entity<HealthyUser>()
                .HasMany(e => e.Dishes)
                .WithMany(e => e.HealthyUsers)
                .UsingEntity<>

        }

    }



}


protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>()
        .HasMany(e => e.Tags)
        .WithMany(e => e.Posts)
        .UsingEntity<PostTag>(
            j => j.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP"));
}