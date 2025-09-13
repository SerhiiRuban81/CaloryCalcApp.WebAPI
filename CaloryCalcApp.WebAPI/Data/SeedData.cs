using CaloryCalcApp.WebAPI.Models.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
                // If our database already have any products, exit
                if (context.Products.Any())
                {
                    return;
                }

                var initialProducts = new List<ProductDTO> { 
                    new ProductDTO {
                        Name = "Raw skinless turkey fillet",
                        // Name = "Філе індички сире без шкіри",
                        Density = 1, // Густина
                        Calories = 105, //kcal
                        Fats = 1, // Жири, г
                        Carbohydrates = 0.1, // Вуглеводи г
                        Proteins = 24 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Drinking water",
                        // Name = "Вода питна",
                        Density = 1, // Густина
                        Calories = 0, //kcal
                        Fats = 0, // Жири, г
                        Carbohydrates = 0, // Вуглеводи г
                        Proteins = 0 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Fresh cucumber",
                        // Name = "Огірок свіжий",
                        Density = 1, // Густина
                        Calories = 15.8, //kcal
                        Fats = 0.18, // Жири, г
                        Carbohydrates = 2.28, // Вуглеводи г
                        Proteins = 0.82 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Tomato",
                        // Name = "Помідор",
                        Density = 1, // Густина
                        Calories = 20, //kcal
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 3.9, // Вуглеводи г
                        Proteins = 0.9 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Boiled chicken egg",
                        // Name = "Яйце куряче варене",
                        Density = 1, // Густина
                        Calories = 143, //kcal
                        Fats = 10.61, // Жири, г
                        Carbohydrates = 1.12, // Вуглеводи г
                        Proteins =  13// Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Banana",
                        // Name = "Банан",
                        Density = 1, // Густина
                        Calories = 94, //kcal
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 22, // Вуглеводи г
                        Proteins = 1.2 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Natural coffee without sugar",
                        // Name = "Кава натуральна без цукру",
                        Density = 1, // Густина
                        Calories = 1.9, //kcal
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 0, // Вуглеводи г
                        Proteins = 0.12 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Boiled buckwheat in water",
                        // Name = "Варена гречка на воді",
                        Density = 1, // Густина
                        Calories = 105, //kcal
                        Fats = 0.62, // Жири, г
                        Carbohydrates = 19.94, // Вуглеводи г
                        Proteins = 3.38 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Watermelon",
                        // Name = "Кавун",
                        Density = 1, // Густина
                        Calories = 28.4, //kcal
                        Fats = 0.17, // Жири, г
                        Carbohydrates = 6, // Вуглеводи г
                        Proteins = 0.65 // Білки, г
                    },
                    new ProductDTO
                    {
                         Name = "Raw chicken egg",
                        // Name = "Яйце куряче сире",
                        Density = 1, // Густина
                        Calories = 151, //kcal
                        Fats = 10.87, // Жири, г
                        Carbohydrates = 0.94, // Вуглеводи г
                        Proteins = 12.38 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "White wheat bread",
                        // Name = "Хліб пшеничний білий",
                        Density = 1, // Густина
                        Calories = 253, //kcal
                        Fats = 4, // Жири, г
                        Carbohydrates = 48, // Вуглеводи г
                        Proteins = 11 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Pink tomatoes",
                        // Name = "Рожеві помідори",
                        Density = 1, // Густина
                        Calories = 22, //kcal
                        Fats = 0.25, // Жири, г
                        Carbohydrates = 3.9, // Вуглеводи г
                        Proteins = 1 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Peaches",
                        // Name = "Персики",
                        Density = 1, // Густина
                        Calories = 46.1, //kcal
                        Fats = 0.2, // Жири, г
                        Carbohydrates = 9.5, // Вуглеводи г
                        Proteins = 0.9 // Білки, г                                  
                    },
                    new ProductDTO
                    {
                        Name = "Fried egg in oil",
                        // Name = "Яйце смажене на олії",
                        Density = 1, // Густина
                        Calories = 175, //kcal
                        Fats = 12.2, // Жири, г
                        Carbohydrates = 0.7, // Вуглеводи г
                        Proteins = 11.12 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Blueberry",
                        // Name = "Лохина",
                        Density = 1, // Густина
                        Calories = 57, //kcal
                        Fats = 0.33, // Жири, г
                        Carbohydrates = 12.09, // Вуглеводи г
                        Proteins = 0.74 // Білки, г
                    },
                    new ProductDTO
                    {
                         Name = "Hard cheese",
                        // Name = "Сир твердий",
                        Density = 1, // Густина
                        Calories = 347, //kcal
                        Fats = 28.5, // Жири, г
                        Carbohydrates = 4.7, // Вуглеводи г
                        Proteins = 22.5 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "White sugar",
                        // Name = "Цукор білий",
                        Density = 1, // Густина
                        Calories = 401, //kcal
                        Fats = 0, // Жири, г
                        Carbohydrates = 100, // Вуглеводи г
                        Proteins = 0 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Fresh avocado",
                        // Name = "Авокадо свіже",
                        Density = 1, // Густина
                        Calories = 159, //kcal
                        Fats = 13.1, // Жири, г
                        Carbohydrates = 6, // Вуглеводи г
                        Proteins = 1.6 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Black tea without sugar",
                        // Name = "Чай чорний без цукру",
                        Density = 1, // Густина
                        Calories = 0, //kcal
                        Fats = 0, // Жири, г
                        Carbohydrates = 0, // Вуглеводи г
                        Proteins = 0 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Boiled chicken fillet",
                        // Name = "Філе куряче варене",
                        Density = 1, // Густина
                        Calories = 151, //kcal
                        Fats = 3, // Жири, г
                        Carbohydrates = 0, // Вуглеводи г
                        Proteins = 29 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Poppy",
                        // Name = "Мак",
                        Density = 1, // Густина
                        Calories = 601, //kcal
                        Fats = 43.8, // Жири, г
                        Carbohydrates = 24.2, // Вуглеводи г
                        Proteins = 20.37 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Baked chicken fillet",
                        // Name = "Куряче філе запечене",
                        Density = 1, // Густина
                        Calories = 165, //kcal
                        Fats = 3.5, // Жири, г
                        Carbohydrates = 0, // Вуглеводи г
                        Proteins = 30 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Nectarine",
                        // Name = "Нектарин",
                        Density = 1, // Густина
                        Calories = 40.8, //kcal
                        Fats = 0.1, // Жири, г
                        Carbohydrates = 8, // Вуглеводи г
                        Proteins = 1.2 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Black bread",
                        // Name = "Хліб чорний",
                        Density = 1, // Густина
                        Calories = 201, //kcal
                        Fats = 1.1, // Жири, г
                        Carbohydrates = 41, // Вуглеводи г
                        Proteins = 6.6 // Білки, г
                    },
                    new ProductDTO
                    {
                        Name = "Butter 82%",
                        // Name = "Масло 82%",
                        Density = 1, // Густина
                        Calories = 744, //kcal
                        Fats = 82, // Жири, г
                        Carbohydrates = 0.8, // Вуглеводи г
                        Proteins = 0.5 // Білки, г
                    },

                    // DRAFT FOR ADDING NEW STARTING PRODUCTS
                    // Чернетка для додавання нових стартових продуктів
                    //new ProductDTO
                    //{
                    //    Name = "",
                    //    // Name = "",
                    //    Density = 1, // Густина
                    //    Calories = , //kcal
                    //    Fats = , // Жири, г
                    //    Carbohydrates = , // Вуглеводи г
                    //    Proteins =  // Білки, г
                    //},

                };




                //Adding products

                context.Products.AddRange(initialProducts);

                await context.SaveChangesAsync();
            }

            
        }
    }
}
