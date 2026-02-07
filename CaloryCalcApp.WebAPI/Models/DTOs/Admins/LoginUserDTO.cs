using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.WebAPI.Models.DTOs.Admin
{
    public class LoginUserDTO
    {
        //public int Id { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string Username { get; set; } = default!;

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Display(Name = "Remain in the system")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
