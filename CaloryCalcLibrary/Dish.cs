using System.ComponentModel.DataAnnotations;

namespace CaloryCalcLibrary
{
    public class Dish
    {
        public int Id { get; set; }

        [Display(Name = "Dish name")]
        public string Name { get; set; } = default!;
    }
}
