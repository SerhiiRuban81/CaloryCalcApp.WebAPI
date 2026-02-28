using CaloryCalcApp.WebAPI.Models.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;

namespace CaloryCalcApp.WebAPI.Data
{
    // Клас для початкової ініціалізації бази даних
    // Дані взято з ресурсів: 
    // https://www.tablycjakalorijnosti.com.ua
    // Class to initialize database with starting data
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
                // This string only for the first run
                context.Database.EnsureCreated();

                //////////////////////////////// Products////////////////////////////////

                // Якщо є будь-які продукти, виходимо
                // If our database already have any products, skipping this part
                if (context.Products.Any())
                {
                    return;
                }

                var initialProducts = new List<Product> {
                    new Product {
                        Name = "Raw skinless turkey fillet",
                        // Name = "Філе індички сире без шкіри",
                        Density = 1.07, // Густина, г/см3
                        Calories = 105, //Калорійність, ккал
                        Fats = 1, // Жири, г
                        Carbohydrates = 0.1, // Вуглеводи, г
                        Proteins = 24 // Білки, г
                    },
                    new Product
                    {
                        Name = "Drinking water",
                        // Name = "Вода питна",
                        Density = 1, // Густина, г/см3
                        Calories = 0, //Калорійність, ккал
                        Fats = 0, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 0 // Білки, г
                    },
                    new Product
                    {
                        Name = "Fresh cucumber",
                        // Name = "Огірок свіжий",
                        Density = 0.8925, // Густина, г/см3
                        Calories = 15.8, //Калорійність, ккал
                        Fats = 0.18, // Жири, г
                        Carbohydrates = 2.28, // Вуглеводи, г
                        Proteins = 0.82 // Білки, г
                    },
                    new Product
                    {
                        Name = "Tomato",
                        // Name = "Помідор",
                        Density = 1.025, // Густина, г/см3
                        Calories = 20, //Калорійність, ккал
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 3.9, // Вуглеводи, г
                        Proteins = 0.9 // Білки, г
                    },
                    new Product
                    {
                        Name = "Boiled chicken egg",
                        // Name = "Яйце куряче варене",
                        Density = 1.037, // Густина, г/см3
                        Calories = 143, //Калорійність, ккал
                        Fats = 10.61, // Жири, г
                        Carbohydrates = 1.12, // Вуглеводи, г
                        Proteins =  13// Білки, г
                    },
                    new Product
                    {
                        Name = "Banana",
                        // Name = "Банан",
                        Density = 1, // Густина, г/см3
                        Calories = 94, //Калорійність, ккал
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 22, // Вуглеводи, г
                        Proteins = 1.2 // Білки, г
                    },
                    new Product
                    {
                        Name = "Natural coffee without sugar",
                        // Name = "Кава натуральна без цукру",
                        Density = 1, // Густина, г/см3
                        Calories = 1.9, //Калорійність, ккал
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 0.12 // Білки, г
                    },
                    new Product
                    {
                        Name = "Boiled buckwheat in water",
                        // Name = "Варена гречка на воді",
                        Density = 0.8, // Густина, г/см3
                        Calories = 105, //Калорійність, ккал
                        Fats = 0.62, // Жири, г
                        Carbohydrates = 19.94, // Вуглеводи, г
                        Proteins = 3.38 // Білки, г
                    },
                    new Product
                    {
                        Name = "Watermelon",
                        // Name = "Кавун",
                        Density = 0.8, // Густина, г/см3
                        Calories = 28.4, //Калорійність, ккал
                        Fats = 0.17, // Жири, г
                        Carbohydrates = 6, // Вуглеводи, г
                        Proteins = 0.65 // Білки, г
                    },
                    new Product
                    {
                         Name = "Raw chicken egg",
                        // Name = "Яйце куряче сире",
                        Density = 1.0825, // Густина, г/см3
                        Calories = 151, //Калорійність, ккал
                        Fats = 10.87, // Жири, г
                        Carbohydrates = 0.94, // Вуглеводи, г
                        Proteins = 12.38 // Білки, г
                    },
                    new Product
                    {
                        Name = "White wheat bread",
                        // Name = "Хліб пшеничний білий",
                        Density = 0.245, // Густина, г/см3
                        Calories = 253, //Калорійність, ккал
                        Fats = 4, // Жири, г
                        Carbohydrates = 48, // Вуглеводи, г
                        Proteins = 11 // Білки, г
                    },
                    new Product
                    {
                        Name = "Pink tomatoes",
                        // Name = "Рожеві помідори",
                        Density = 0.6, // Густина, г/см3
                        Calories = 22, //Калорійність, ккал
                        Fats = 0.25, // Жири, г
                        Carbohydrates = 3.9, // Вуглеводи, г
                        Proteins = 1 // Білки, г
                    },
                    new Product
                    {
                        Name = "Peaches",
                        // Name = "Персики",
                        Density = 0.9, // Густина, г/см3
                        Calories = 46.1, //Калорійність, ккал
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 9.5, // Вуглеводи, г
                        Proteins = 0.9 // Білки, г                                  
                    },
                    new Product
                    {
                        Name = "Fried egg in oil",
                        // Name = "Яйце смажене на олії",
                        Density = 0.95, // Густина, г/см3
                        Calories = 175, //Калорійність, ккал
                        Fats = 12.2, // Жири, г
                        Carbohydrates = 0.7, // Вуглеводи, г
                        Proteins = 11.12 // Білки, г
                    },
                    new Product
                    {
                        Name = "Blueberry",
                        // Name = "Лохина",
                        Density = 1.02, // Густина, г/см3
                        Calories = 57, //Калорійність, ккал
                        Fats = 0.33, // Жири, г
                        Carbohydrates = 12.09, // Вуглеводи, г
                        Proteins = 0.74 // Білки, г
                    },
                    new Product
                    {
                         Name = "Hard cheese",
                        // Name = "Сир твердий",
                        Density = 1.1, // Густина, г/см3
                        Calories = 347, //Калорійність, ккал
                        Fats = 28.5, // Жири, г
                        Carbohydrates = 4.7, // Вуглеводи, г
                        Proteins = 22.5 // Білки, г
                    },
                    new Product
                    {
                        Name = "White sugar",
                        // Name = "Цукор білий",
                        Density = 0.88, // Густина, г/см3
                        Calories = 401, //Калорійність, ккал
                        Fats = 0, // Жири, г
                        Carbohydrates = 100, // Вуглеводи, г
                        Proteins = 0 // Білки, г
                    },
                    new Product
                    {
                        Name = "Fresh avocado",
                        // Name = "Авокадо свіже",
                        Density = 0.94, // Густина, г/см3
                        Calories = 159, //Калорійність, ккал
                        Fats = 13.1, // Жири, г
                        Carbohydrates = 6, // Вуглеводи, г
                        Proteins = 1.6 // Білки, г
                    },
                    new Product
                    {
                        Name = "Black tea without sugar",
                        // Name = "Чай чорний без цукру",
                        Density = 1, // Густина, г/см3
                        Calories = 0, //Калорійність, ккал
                        Fats = 0, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 0 // Білки, г
                    },
                    new Product
                    {
                        Name = "Boiled chicken fillet",
                        // Name = "Філе куряче варене",
                        Density = 1.065, // Густина, г/см3
                        Calories = 151, //Калорійність, ккал
                        Fats = 3, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 29 // Білки, г
                    },
                    new Product
                    {
                        Name = "Poppy",
                        // Name = "Мак",
                        Density = 0.6, // Густина, г/см3
                        Calories = 601, //Калорійність, ккал
                        Fats = 43.8, // Жири, г
                        Carbohydrates = 24.2, // Вуглеводи, г
                        Proteins = 20.37 // Білки, г
                    },
                    new Product
                    {
                        Name = "Baked chicken fillet",
                        // Name = "Куряче філе запечене",
                        Density = 1.125, // Густина, г/см3
                        Calories = 165, //Калорійність, ккал
                        Fats = 3.5, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 30 // Білки, г
                    },
                    new Product
                    {
                        Name = "Nectarine",
                        // Name = "Нектарин",
                        Density = 1.04, // Густина, г/см3
                        Calories = 40.8, //Калорійність, ккал
                        Fats = 0.1, // Жири, г
                        Carbohydrates = 8, // Вуглеводи, г
                        Proteins = 1.2 // Білки, г
                    },
                    new Product
                    {
                        Name = "Black bread",
                        // Name = "Хліб чорний",
                        Density = 0.475, // Густина, г/см3
                        Calories = 201, //Калорійність, ккал
                        Fats = 1.1, // Жири, г
                        Carbohydrates = 41, // Вуглеводи, г
                        Proteins = 6.6 // Білки, г
                    },
                    new Product
                    {
                        Name = "Butter 82%",
                        // Name = "Масло 82%",
                        Density = 0.91, // Густина, г/см3 г/см3
                        Calories = 744, //Калорійність, ккал
                        Fats = 82, // Жири, г
                        Carbohydrates = 0.8, // Вуглеводи, г
                        Proteins = 0.5 // Білки, г
                    },

                    /////////////////////////////////
                    /// PRODUCTS FOR BORSCH ///
                    ///////////////////////////////// 
                    new Product
                    {
                        Name = "Raw onion",
                        // Name = "Цибуля сира",
                        Density = 0.86, // Густина, г/см3, г/см3
                        Calories = 42, //Калорійність, ккал
                        Fats = 0.1, // Жири, г
                        Carbohydrates = 10.1, // Вуглеводи, г
                        Proteins = 0.92 // Білки, г
                    },
                    new Product
                    {
                        Name = "Raw carrot",
                        // Name = "Морква сира",
                        Density = 1.04, // Густина, г/см3, г/см3
                        Calories = 41, //Калорійність, ккал
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 9.6, // Вуглеводи, г
                        Proteins = 0.9 // Білки, г
                    },
                    new Product
                    {
                        Name = "Raw beetroot",
                        // Name = "Буряк сирий",
                        Density = 1.05, // Густина, г/см3, г/см3
                        Calories = 43, //Калорійність, ккал
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 9.6, // Вуглеводи, г
                        Proteins = 1.6 // Білки, г
                    },
                    new Product
                    {
                        Name = "Raw potato",
                        // Name = "Картопля сира",
                        Density = 1.07, // Густина, г/см3, г/см3
                        Calories = 77, //Калорійність, ккал
                        Fats = 0.1, // Жири, г
                        Carbohydrates = 17.5, // Вуглеводи, г
                        Proteins = 2 // Білки, г
                    },
                    new Product
                    {
                        Name = "Raw white cabbage",
                        // Name = "Капуста білокачанна сира",
                        Density = 0.92, // Густина, г/см3, г/см3
                        Calories = 24, //Калорійність, ккал
                        Fats = 0.1, // Жири, г
                        Carbohydrates = 5.4, // Вуглеводи, г
                        Proteins = 1.2 // Білки, г
                    },
                    new Product
                    {
                        Name = "Tomato paste",
                        // Name = "Томатна паста",
                        Density = 1.13, // Густина, г/см3, г/см3
                        Calories = 82, //Калорійність, ккал
                        Fats = 0.5, // Жири, г
                        Carbohydrates = 18.9, // Вуглеводи, г
                        Proteins = 4.3 // Білки, г
                    },
                    new Product
                    {
                        Name = "Sunflower oil",
                        // Name = "Соняшникова олія",
                        Density = 0.92, // Густина, г/см3, г/см3
                        Calories = 884, //Калорійність, ккал
                        Fats = 100, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 0 // Білки, г
                    },
                    new Product
                    {
                        Name = "Bay leaf",
                        // Name = "Лавровий лист",
                        Density = 0.37, // Густина, г/см3, г/см3
                        Calories = 313, //Калорійність, ккал
                        Fats = 8.4, // Жири, г
                        Carbohydrates = 75, // Вуглеводи, г
                        Proteins = 7.6 // Білки, г
                    },
                    new Product
                    {
                        Name = "Raw pork meat with bone",
                        // Name = "М'ясо свинина сира з кісткою",
                        Density = 0.93, // Густина, г/см3, г/см3
                        Calories = 242, //Калорійність, ккал
                        Fats = 17.0, // Жири, г
                        Carbohydrates = 0, // Вуглеводи, г
                        Proteins = 27.3 // Білки, г
                    },
                    ////////////////////////////////////////////////////
                    




                    // DRAFT FOR ADDING NEW STARTING PRODUCTS
                    // Чернетка для додавання нових стартових продуктів
                    //new Product
                    //{
                    //    Name = "",
                    //    // Name = "",
                    //    Density = 1, // Густина, г/см3, г/см3
                    //    Calories = , //Калорійність, ккал
                    //    Fats = , // Жири, г
                    //    Carbohydrates = , // Вуглеводи, г
                    //    Proteins =  // Білки, г
                    //},

                };
                //Adding products
                context.Products.AddRange(initialProducts);

                ////////////////////// ADDING DISHES //////////////////////
                /// Let's add BORSCH ///
                /// 
                //var initialDishes = new List<Dish>
                //{
                //    new Dish
                //    {
                //        Name = "Borsch",
                //        // Name = "Борщ",
                //        Products =new List<Product>
                //        {
                //            new DishProduct
                //            {

                //            }

                //        },
                //    },

                //};
                //context.Dishes.AddRange(initialDishes);


                // Adding default roles
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
