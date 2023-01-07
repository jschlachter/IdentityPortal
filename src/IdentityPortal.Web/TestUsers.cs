using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace IdentityPortal;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new {
                street_address = "101 Some Street N",
                city = "Minneapolis",
                postal_code = 55412,
                state = "Minnesota"
            };

            return new List<TestUser> {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob TestUser"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "TestUser"),
                        new Claim(JwtClaimTypes.Email, "bob.testuser@gmail.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://mywebsite.com"),
                        new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                    }
                }
            };
        }
    }
}
