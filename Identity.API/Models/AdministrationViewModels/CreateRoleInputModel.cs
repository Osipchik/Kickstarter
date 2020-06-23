using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AdministrationViewModels
{
    public class CreateRoleInputModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}