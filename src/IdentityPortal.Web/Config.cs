using Duende.IdentityServer.Models;

namespace IdentityPortal;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(name: "", displayName: "")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "portal",
                AllowedGrantTypes = GrantTypes.Hybrid,
                ClientSecrets =
                {
                    new Secret("".Sha256())
                },
                RedirectUris = { "" },
                PostLogoutRedirectUris = { "" },
                AllowedScopes = { "api1" }
            }
        };
}
