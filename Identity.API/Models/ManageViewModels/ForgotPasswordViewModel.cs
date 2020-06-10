using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.ManageViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}