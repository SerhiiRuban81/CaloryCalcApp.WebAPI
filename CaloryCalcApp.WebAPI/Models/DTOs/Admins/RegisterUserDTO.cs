using CaloryCalcLibrary;
using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.Web.Models.DTOs.Admins
{
    public class RegisterUserDto
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
        public double Weight { get; set; } = default!;

        [Required]
        [Display(Name = "Height")]
        public int Height { get; set; } = default!;

        [Required]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Sex")]
        public Sex Sex { get; set; } = default!;

        [Display(Name = "Remain in system")]
        public bool IsPersistent { get; set; } = false;
    }
}
