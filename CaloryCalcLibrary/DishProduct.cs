using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    internal class DishProduct
    {
        public int id {  get; set; }

        [ForeignKey(nameof(Product))]
        public int productId { get; set; }

        [ForeignKey(nameof(Dish))]
        public int dishId { get; set; }

        public string measurementUnit { get; set; } = default!;

        public float amount { get; set; }
    }
}
