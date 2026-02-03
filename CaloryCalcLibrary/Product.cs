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
        public string Name { get; set; } = default!; // Назва продукту

        [Range(0, double.MaxValue)]
        public double? Density { get; set; } // Щільність продукту (г/мл)

        [Range(0, double.MaxValue)]
        public double? Calories { get; set; } // Калорійність на 100 г або 100 мл

        [Range(0, double.MaxValue)]
        public double? Fats { get; set; } // Жири на 100 г або мл

        [Range(0, double.MaxValue)]
        public double? Carbohydrates { get; set; } // Вуглеводи на 100 г або мл

        [Range(0, double.MaxValue)]
        public double? Proteins { get; set; } // Білки на 100 г або мл

        public List<Dish> Dishes { get; set; } = []; // Список страв, що містять цей продукт

        public List<DishProduct> DishProducts { get; set; } = []; // Список зв'язків між стравами та продуктом

    }
}
