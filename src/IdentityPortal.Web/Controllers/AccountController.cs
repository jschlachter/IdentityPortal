using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Duende.IdentityServer.Test;
using IdentityPortal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityPortal.Controllers;

[AllowAnonymous]
public class AccountController : Controller
{
    readonly IIdentityServerInteractionService _interaction;
    readonly IEventService _events;
    readonly IAuthenticationSchemeProvider _schemeProvider;
    readonly IIdentityProviderStore _identityProviderStore;
    readonly ILogger<AccountController> _logger;

    public AccountController (IIdentityServerInteractionService interaction,
        IAuthenticationSchemeProvider schemeProvider,
        IIdentityProviderStore identityProviderStore,
        IEventService events,
        TestUserStore? users,
        ILogger<AccountController> logger)
    {
        _interaction = interaction;
        _events = events;
        _schemeProvider = schemeProvider;
        _identityProviderStore = identityProviderStore;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string returnUrl)
    {
        var model = await BuildModelAsync(returnUrl);

        if (model.IsExternalLoginOnly)
        {
            // we only have one option for logging in and it's an external provider
            return RedirectToPage("/ExternalLogin/Challenge", new { scheme = model.ExternalLoginScheme, returnUrl });
        }

        return View(model);
    }

    async Task<LoginViewModel> BuildModelAsync(string returnUrl)
    {
        var login = new LoginModel()
        {
            ReturnUrl = returnUrl
        };

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var viewModel = new LoginViewModel {
                EnableLocalLogin = local,
            };

            login.Username = context?.LoginHint;

            if (!local)
            {
                viewModel.ExternalProviders = new[] {
                    new LoginViewModel.ExternalProvider { AuthenticationScheme = context.IdP }
                };
            }
        }

        var schemes = await _schemeProvider.GetAllSchemesAsync();

        var providers = schemes
            .Where(x => x.DisplayName != null)
            .Select(x => new LoginViewModel.ExternalProvider
            {
                DisplayName = x.DisplayName ?? x.Name,
                AuthenticationScheme = x.Name
            }).ToList();

        var dyanmicSchemes = (await _identityProviderStore.GetAllSchemeNamesAsync())
            .Where(x => x.Enabled)
            .Select(x => new LoginViewModel.ExternalProvider
            {
                AuthenticationScheme = x.Scheme,
                DisplayName = x.DisplayName
            });
        providers.AddRange(dyanmicSchemes);


        var allowLocal = true;
        var client = context?.Client;
        if (client != null)
        {
            allowLocal = client.EnableLocalLogin;
            if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
            {
                providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
            }
        }

        return new LoginViewModel {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
            EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
            ExternalProviders = providers.ToArray()
        };
    }
}
