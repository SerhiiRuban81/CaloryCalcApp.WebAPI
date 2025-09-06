using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Product")]
        public string Name { get; set; } = default!;

        public double Density { get; set; }

        public double Calories { get; set; }

        public double Fats { get; set; }

        public double Carbohydrates { get; set; }

        public double Proteins { get; set; }

        public List<Dish> Dishes { get; set; } = [];

        public List<DishProduct> DishProducts { get; set; } = [];

    }
}
