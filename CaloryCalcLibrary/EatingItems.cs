using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    internal class EatingItems
    {
        public int id {  get; set; }

        [ForeignKey(nameof(Dish))]
        public int dishId { get; set; }

        public float amount { get; set; }

        [ForeignKey(nameof(User))]
        public int userId { get; set; }
    }
}
