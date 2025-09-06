using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalcLibrary
{
    public class HealthyUser : IdentityUser
    {
        public double Weight { get; set; } = default!; // Вага

        public int Height { get; set; } = default!; // Зріст

        public DateOnly DateOfBirth { get; set; } // Дата народження

    }
}
