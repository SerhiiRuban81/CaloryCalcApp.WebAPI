using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    internal class Product
    {
        public int id { get; set; }

        [Display(Name = "Product")]
        public string Name { get; set; } = default!;

        public float density { get; set; }

        public float calories { get; set; }

        public float fats { get; set; }

        public float carbohydrates { get; set; }

        public float proteins { get; set; }

    }
}
