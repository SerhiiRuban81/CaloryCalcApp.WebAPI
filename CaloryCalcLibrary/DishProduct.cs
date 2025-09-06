using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    public class DishProduct
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [ForeignKey(nameof(Dish))]
        public int DishId { get; set; }

        public string MeasurementUnit { get; set; } = default!;

        public float Amount { get; set; }

        public Dish Dish { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
