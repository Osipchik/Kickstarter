using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventBus.Events;
using Identity.API.Configuration;
using Identity.API.Data;
using Identity.API.Extensions;
using Identity.API.Models.AccountViewModels;
using Identity.API.Models.ManageViewModels;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApplicationUser = Identity.API.Models.ApplicationUser;

namespace Identity.API.Controllers
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IBusControl _bus;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IBusControl bus)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _bus = bus;
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var vm = new RegisterInputModel{ReturnUrl = returnUrl};

            return View("Register", vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser{Email = model.UserName, Name = model.Name, UserName = model.UserName};
                
                var result = await _userManager.CreateAsync(user, model.Password);
            
                if (result.Succeeded)
                {
                    await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, user.Name));
                    await _userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Email, user.Email));
                    await _userManager.AddToRoleAsync(user, DbInit.MaiRole);
                    
                    await SendEmail(user, "ConfirmAccount");
                    
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                }
                ModelState.AddModelError("UserName", "This email is already exist.");
            }

            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

            return View("Register", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user ID {userId} is invalid";
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View("Error");
        }
        
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            return View("Login", await BuildLoginViewModelAsync(returnUrl));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRegisterInputModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
        
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
                    
                    var props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                    };
                    
                    var issuer = new IdentityServerUser(user.Id)
                    {
                        DisplayName = user.Name
                    };
                    
                    await HttpContext.SignInAsync(issuer, props);
        
                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.Client.ClientId))
                        {
                            return View("Redirect", new RedirectViewModel { RedirectUrl = model.ReturnUrl });
                        }
        
                        return Redirect(model.ReturnUrl);
                    }
        
                    if (Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
        
                    throw new Exception("invalid return URL");
                }
        
                await _events.RaiseAsync(new UserLoginFailureEvent(model.UserName, "invalid credentials", clientId:context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
            }
        
            return View("Login", await BuildLoginViewModelAsync(model));
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                return await Logout(vm);
            }

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync();
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
                await _signInManager.SignOutAsync();
            }

            if (vm.TriggerExternalSignout)
            {
                var url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword(string returnUrl)
        {
            return View(new ForgotPasswordViewModel{ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    await SendEmail(user, "PasswordReset");
                    return View();
                }
                
                return Redirect(model.ReturnUrl);
            }
            
            ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (!string.IsNullOrEmpty(userId) || !string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            
            return View(new ResetPasswordViewModel{Token = token, UserId = userId});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                return RedirectToAction("Index", "Home");
            }
            
            return View(model);
        }
        
        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    UserName = context.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider =>
                            client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                UserName = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginRegisterInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.UserName = model.UserName;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

            if (User?.Identity.IsAuthenticated != true)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                vm.ShowLogoutPrompt = false;
                return vm;
            }
            
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if (User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignOut = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignOut)
                    {
                        vm.LogoutId ??= await _interaction.CreateLogoutContextAsync();
                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }

        private async Task SendEmail(ApplicationUser user, string type)
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri($"rabbitmq://localhost/MailService"));
            await endpoint.Send(await BuildMailMessage(user, type));
        }

        private ValueTask<MailMessage> BuildMailMessage(ApplicationUser user, string type)
        {
            return type switch
            {
                "ConfirmAccount" => BuildConfirmEmail(user),
                "PasswordReset" => BuildPasswordResetEmail(user),
            };
        }
        
        private async ValueTask<MailMessage> BuildConfirmEmail(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail", "Account",
                new { userId = user.Id, token = token }, HttpContext.Request.Scheme);
            
            return new MailMessage(user.Email, user.Name, callbackUrl, "ConfirmAccount");
        }
        
        private async ValueTask<MailMessage> BuildPasswordResetEmail(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                "ResetPassword", "Account",
                new { userId = user.Id, token = token }, HttpContext.Request.Scheme);
            
            return new MailMessage(user.Email, user.Name, callbackUrl, "PasswordReset");
        }
    }
}