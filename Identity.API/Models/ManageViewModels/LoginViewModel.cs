using System.Collections.Generic;
using System.Linq;
using Identity.API.Models.AccountViewModels;

namespace Identity.API.Models.ManageViewModels
{
    public class LoginViewModel : LoginInputModel
    {
        public bool EnableLocalLogin { get; set; } = true;

        public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));
    }
}