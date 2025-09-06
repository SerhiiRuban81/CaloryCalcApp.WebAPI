using System.ComponentModel.DataAnnotations;

namespace CaloryCalcLibrary
{
    public class Dish
    {
        public int Id { get; set; }

        [Display(Name = "Dish name")]
        public string Name { get; set; } = default!;

        public List<Product> Products { get; set; } = [];

        public List<DishProduct> DishProducts { get; set; } = [];

        public List<HealthyUser> HealthyUsers { get; set; } = [];
    }
}
