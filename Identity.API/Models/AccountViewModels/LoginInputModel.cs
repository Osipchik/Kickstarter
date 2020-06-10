using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModels
{
    public class LoginInputModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}