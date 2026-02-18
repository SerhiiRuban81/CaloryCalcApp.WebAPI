using CaloryCalcApp.WebAPI.Models.DTOs.Admin;
using CaloryCalcApp.WebAPI.Models.DTOs.Admins;
using CaloryCalcApp.WebAPI.Models.DTOs.Dishes;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProducts;
using CaloryCalcApp.WebAPI.Models.DTOs.Products;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using CaloryCalcApp.WebAPI.Models.DTOs.Admins;
using CaloryCalcApp.WebAPI.Models.DTOs.Admin;
using CaloryCalcApp.WebAPI.Models.DTOs.HealthyUsers;
using System.Reflection.Emit;

namespace CaloryCalcApp.WebAPI.Data
{
	public class CaloriesContext : IdentityDbContext<HealthyUser>
	{
		//public int Id { get; set; }

		public CaloriesContext(DbContextOptions<CaloriesContext> options) : base(options) { }

		public DbSet<Dish> Dishes { get; set; }

		public DbSet<DishProduct> DishProducts { get; set; }

		public DbSet<HealthyUserDish> HealthyUserDishes { get; set; }

		public DbSet<Product> Products { get; set; }

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
				.UsingEntity<HealthyUserDish>(e =>
				e.Property(c => c.MealTime).HasDefaultValueSql("GETUTCDATE()"));
		}
		//public DbSet<CaloryCalcApp.WebAPI.Models.DTOs.Products.ProductDTO> ProductDTO { get; set; } = default!;
		//public DbSet<CaloryCalcApp.WebAPI.Models.DTOs.Admins.RegisterUserDTO> RegisterUserDTO { get; set; } = default!;
		//public DbSet<CaloryCalcApp.WebAPI.Models.DTOs.Admin.LoginUserDTO> LoginUserDTO { get; set; } = default!;
		//public DbSet<CaloryCalcApp.WebAPI.Models.DTOs.Admins.RegisterUserDTO> RegisterUserDTO { get; set; } = default!;

	}
}
