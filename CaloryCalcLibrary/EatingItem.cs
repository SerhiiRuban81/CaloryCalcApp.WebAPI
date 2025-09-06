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
        
        public int DishId { get; set; }

        public float Amount { get; set; }

        public int HealthyUserId { get; set; }

        public DateTime MealTime { get; set; }        

    }
}
