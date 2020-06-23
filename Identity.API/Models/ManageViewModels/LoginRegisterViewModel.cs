using System.Collections.Generic;
using System.Linq;
using Identity.API.Models.AccountViewModels;

namespace Identity.API.Models.ManageViewModels
{
    public class LoginRegisterViewModel : LoginRegisterInputModel
    {
        public bool IsRegisterRequest { get; set; }
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();

        public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    }
}