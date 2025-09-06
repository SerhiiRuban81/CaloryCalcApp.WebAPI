using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    public class EatingItem
    {
        public int Id {  get; set; }

        [ForeignKey(nameof(Dish))]
        public int DishId { get; set; }

        public float Amount { get; set; }

        [ForeignKey(nameof(HealthyUser))]
        public int UserId { get; set; }

        public DateTime MealTime { get; set; }
    }
}
