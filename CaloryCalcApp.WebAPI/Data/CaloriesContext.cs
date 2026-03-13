using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalcApp.Web.Data
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
	    //public DbSet<CaloryCalcApp.Web.Models.DTOs.Roles.RoleDto> RoleDto { get; set; } = default!;
	    //public DbSet<CaloryCalcApp.Web.Models.DTOs.Roles.RoleDto> RoleDto { get; set; } = default!;
		//public DbSet<CaloryCalcApp.Web.Models.DTOs.Products.ProductDto> ProductDto { get; set; } = default!;
		//public DbSet<CaloryCalcApp.Web.Models.DTOs.Admins.RegisterUserDto> RegisterUserDto { get; set; } = default!;
		//public DbSet<CaloryCalcApp.Web.Models.DTOs.Admin.LoginUserDto> LoginUserDto { get; set; } = default!;
		//public DbSet<CaloryCalcApp.Web.Models.DTOs.Admins.RegisterUserDto> RegisterUserDto { get; set; } = default!;

	}
}

