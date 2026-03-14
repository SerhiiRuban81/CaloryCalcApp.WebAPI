using System.ComponentModel.DataAnnotations;

namespace CaloryCalcApp.Application.DTOs.Roles
{
    public class RoleDto
    {
        public string Id { get; set; } = default!;

        [Display(Name = "Role title")]
        public string Name { get; set; } = default!;
    }
}
