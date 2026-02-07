using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.WebAPI.Models.DTOs.Users
{
    public class CaloryCalcUserDTO
    {
        public string Id { get; set; } = default!;

        [Display(Name = "Login")]
        public string UserName { get; set; } = default!;

        [Display(Name = "Email")]
        public string Email { get; set; } = default!;



    }
}
