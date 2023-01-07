using Duende.IdentityServer.Models;

namespace IdentityPortal;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("Scope1")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "portal",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = false,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RedirectUris = { "https://localhost:8080/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:8080/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:8080/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope1" }
            }
        };
}
