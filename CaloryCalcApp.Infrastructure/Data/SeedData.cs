using CaloryCalcLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaloryCalcApp.Infrastructure.Data
{
    public class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration)
        {
            DbContextOptions<CaloriesContext> options =
                serviceProvider.GetRequiredService<DbContextOptions<CaloriesContext>>();

            using (CaloriesContext context = new CaloriesContext(options))
            {
                context.Database.EnsureCreated();

                if (context.Products.Any())
                {
                    return;
                }

                var initialProducts = new List<Product> {
                    new Product {
                        Name = "Raw skinless turkey fillet",
                        Density = 1.07,
                        Calories = 105,
                        Fats = 1,
                        Carbohydrates = 0.1,
                        Proteins = 24
                    },
                    new Product
                    {
                        Name = "Drinking water",
                        Density = 1,
                        Calories = 0,
                        Fats = 0,
                        Carbohydrates = 0,
                        Proteins = 0
                    },
                    new Product
                    {
                        Name = "Fresh cucumber",
                        Density = 0.8925,
                        Calories = 15.8,
                        Fats = 0.18,
                        Carbohydrates = 2.28,
                        Proteins = 0.82
                    },
                    new Product
                    {
                        Name = "Tomato",
                        Density = 1.025,
                        Calories = 20,
                        Fats = 0.2,
                        Carbohydrates = 3.9,
                        Proteins = 0.9
                    },
                    new Product
                    {
                        Name = "Boiled chicken egg",
                        Density = 1.037,
                        Calories = 143,
                        Fats = 10.61,
                        Carbohydrates = 1.12,
                        Proteins = 13
                    },
                    new Product
                    {
                        Name = "Banana",
                        Density = 1,
                        Calories = 94,
                        Fats = 0.2,
                        Carbohydrates = 22,
                        Proteins = 1.2
                    },
                    new Product
                    {
                        Name = "Natural coffee without sugar",
                        Density = 1,
                        Calories = 1.9,
                        Fats = 0.2,
                        Carbohydrates = 0,
                        Proteins = 0.12
                    },
                    new Product
                    {
                        Name = "Boiled buckwheat in water",
                        Density = 0.8,
                        Calories = 105,
                        Fats = 0.62,
                        Carbohydrates = 19.94,
                        Proteins = 3.38
                    },
                    new Product
                    {
                        Name = "Watermelon",
                        Density = 0.8,
                        Calories = 28.4,
                        Fats = 0.17,
                        Carbohydrates = 6,
                        Proteins = 0.65
                    },
                    new Product
                    {
                        Name = "Raw chicken egg",
                        Density = 1.0825,
                        Calories = 151,
                        Fats = 10.87,
                        Carbohydrates = 0.94,
                        Proteins = 12.38
                    },
                    new Product
                    {
                        Name = "White wheat bread",
                        Density = 0.245,
                        Calories = 253,
                        Fats = 4,
                        Carbohydrates = 48,
                        Proteins = 11
                    },
                    new Product
                    {
                        Name = "Pink tomatoes",
                        Density = 0.6,
                        Calories = 22,
                        Fats = 0.25,
                        Carbohydrates = 3.9,
                        Proteins = 1
                    },
                    new Product
                    {
                        Name = "Peaches",
                        Density = 0.9,
                        Calories = 46.1,
                        Fats = 0.2,
                        Carbohydrates = 9.5,
                        Proteins = 0.9
                    },
                    new Product
                    {
                        Name = "Fried egg in oil",
                        Density = 0.95,
                        Calories = 175,
                        Fats = 12.2,
                        Carbohydrates = 0.7,
                        Proteins = 11.12
                    },
                    new Product
                    {
                        Name = "Blueberry",
                        Density = 1.02,
                        Calories = 57,
                        Fats = 0.33,
                        Carbohydrates = 12.09,
                        Proteins = 0.74
                    },
                    new Product
                    {
                        Name = "Hard cheese",
                        Density = 1.1,
                        Calories = 347,
                        Fats = 28.5,
                        Carbohydrates = 4.7,
                        Proteins = 22.5
                    },
                    new Product
                    {
                        Name = "White sugar",
                        Density = 0.88,
                        Calories = 401,
                        Fats = 0,
                        Carbohydrates = 100,
                        Proteins = 0
                    },
                    new Product
                    {
                        Name = "Fresh avocado",
                        Density = 0.94,
                        Calories = 159,
                        Fats = 13.1,
                        Carbohydrates = 6,
                        Proteins = 1.6
                    },
                    new Product
                    {
                        Name = "Black tea without sugar",
                        Density = 1,
                        Calories = 0,
                        Fats = 0,
                        Carbohydrates = 0,
                        Proteins = 0
                    },
                    new Product
                    {
                        Name = "Boiled chicken fillet",
                        Density = 1.065,
                        Calories = 151,
                        Fats = 3,
                        Carbohydrates = 0,
                        Proteins = 29
                    },
                    new Product
                    {
                        Name = "Poppy",
                        Density = 0.6,
                        Calories = 601,
                        Fats = 43.8,
                        Carbohydrates = 24.2,
                        Proteins = 20.37
                    },
                    new Product
                    {
                        Name = "Baked chicken fillet",
                        Density = 1.125,
                        Calories = 165,
                        Fats = 3.5,
                        Carbohydrates = 0,
                        Proteins = 30
                    },
                    new Product
                    {
                        Name = "Nectarine",
                        Density = 1.04,
                        Calories = 40.8,
                        Fats = 0.1,
                        Carbohydrates = 8,
                        Proteins = 1.2
                    },
                    new Product
                    {
                        Name = "Black bread",
                        Density = 0.475,
                        Calories = 201,
                        Fats = 1.1,
                        Carbohydrates = 41,
                        Proteins = 6.6
                    },
                    new Product
                    {
                        Name = "Butter 82%",
                        Density = 0.91,
                        Calories = 744,
                        Fats = 82,
                        Carbohydrates = 0.8,
                        Proteins = 0.5
                    },
                    new Product
                    {
                        Name = "Raw onion",
                        Density = 0.86,
                        Calories = 42,
                        Fats = 0.1,
                        Carbohydrates = 10.1,
                        Proteins = 0.92
                    },
                    new Product
                    {
                        Name = "Raw carrot",
                        Density = 1.04,
                        Calories = 41,
                        Fats = 0.2,
                        Carbohydrates = 9.6,
                        Proteins = 0.9
                    },
                    new Product
                    {
                        Name = "Raw beetroot",
                        Density = 1.05,
                        Calories = 43,
                        Fats = 0.2,
                        Carbohydrates = 9.6,
                        Proteins = 1.6
                    },
                    new Product
                    {
                        Name = "Raw potato",
                        Density = 1.07,
                        Calories = 77,
                        Fats = 0.1,
                        Carbohydrates = 17.5,
                        Proteins = 2
                    },
                    new Product
                    {
                        Name = "Raw white cabbage",
                        Density = 0.92,
                        Calories = 24,
                        Fats = 0.1,
                        Carbohydrates = 5.4,
                        Proteins = 1.2
                    },
                    new Product
                    {
                        Name = "Tomato paste",
                        Density = 1.13,
                        Calories = 82,
                        Fats = 0.5,
                        Carbohydrates = 18.9,
                        Proteins = 4.3
                    },
                    new Product
                    {
                        Name = "Sunflower oil",
                        Density = 0.92,
                        Calories = 884,
                        Fats = 100,
                        Carbohydrates = 0,
                        Proteins = 0
                    },
                    new Product
                    {
                        Name = "Bay leaf",
                        Density = 0.37,
                        Calories = 313,
                        Fats = 8.4,
                        Carbohydrates = 75,
                        Proteins = 7.6
                    },
                    new Product
                    {
                        Name = "Raw pork meat with bone",
                        Density = 0.93,
                        Calories = 242,
                        Fats = 17.0,
                        Carbohydrates = 0,
                        Proteins = 27.3
                    },
                };

                context.Products.AddRange(initialProducts);

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "admin", "user", "manager" };
                foreach (string role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
