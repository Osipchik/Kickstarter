using Identity.API.Models.AccountViewModels;

namespace Identity.API.Models.ManageViewModels
{
    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
