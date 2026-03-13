using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.Web.Models.DTOs.Users
{
    public class CaloryCalcUserDto
    {
        public string Id { get; set; } = default!;

        [Display(Name = "Login")]
        public string UserName { get; set; } = default!;

        [Display(Name = "Email")]
        public string Email { get; set; } = default!;



    }
}

