using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    public class HealthyUserDishDTO
    {
        public int Id {  get; set; }
        
        public int DishId { get; set; } // Foreign key to Dish

        public float Amount { get; set; } // Amount of the dish consumed

        public string HealthyUserId { get; set; } = default!;

        public DateTime MealTime { get; set; }

        public HealthyUser HealthyUser { get; set; } = null!;
        public Dish Dish { get; set; } = null!;

    }
}
