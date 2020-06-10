using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
        
        public string Token { get; set; }
        public string ReturnUrl { get; set; }
    }
}