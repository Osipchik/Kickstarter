using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.API.Extensions;
using Identity.API.Infrastructure;
using Identity.API.Models.ManageViewModels;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API.Controllers
{
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IEventService _events;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService events,
            ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = events;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var scopesConsented = resources.IdentityResources
                .Select(x => x.Name).ToList();
            
            scopesConsented.AddRange(resources.ApiResources.Select(x => x.Name));
            
            var result = await ProcessConsent(returnUrl, scopesConsented);
            
            if (result.IsRedirect)
            {
                if (await _clientStore.IsPkceClientAsync(result.ClientId))
                {
                    return View("Redirect", new RedirectViewModel { RedirectUrl = result.RedirectUri });
                }

                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError(string.Empty, result.ValidationError);
            }

            return View("Error");
        }
        

        /*****************************************/
        /* helper APIs for the ConsentController */
        /*****************************************/
        private async Task<ProcessConsentResult> ProcessConsent(string returnUrl, IEnumerable<string> scopesConsented)
        {
            var result = new ProcessConsentResult();

            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request == null) return result;

            ConsentResponse grantedConsent = null;
            
            var scopes = scopesConsented;

            grantedConsent = new ConsentResponse
            {
                RememberConsent = true,
                ScopesConsented = scopes
            };

            await _events.RaiseAsync(new ConsentGrantedEvent(
                User.GetSubjectId(),
                request.ClientId,
                request.ScopesRequested,
                grantedConsent.ScopesConsented,
                grantedConsent.RememberConsent));
            
            await _interaction.GrantConsentAsync(request, grantedConsent);

            result.RedirectUri = returnUrl;
            result.ClientId = request.ClientId;

            return result;
        }
    }
}