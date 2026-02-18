using CaloryCalcLibrary;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.WebAPI.Models.DTOs.Admins
{
   

    public class RegisterUserDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full name")]
        public string Username { get; set; } = default!;

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required]
        [Display(Name = "Password confirmation")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = default!;


        [Required]
        [Display(Name = "Weight")]
        public double Weight { get; set; } = default!; // Вага

        [Required]
        [Display(Name = "Height")]
        public int Height { get; set; } = default!; // Зріст

        [Required]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; } // Дата народження

        [Required]
        [Display(Name = "Sex")]
        public Sex Sex { get; set; } = default!; // Стать

        // To remain in system or exit
        [Display(Name = "Remain in system")]
        public bool IsPersistent { get; set; } = false;
    }
}
