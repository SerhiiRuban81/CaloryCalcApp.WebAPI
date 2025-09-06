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

        public float Density { get; set; }

        public float Calories { get; set; }

        public float Fats { get; set; }

        public float Carbohydrates { get; set; }

        public float Proteins { get; set; }

    }
}
