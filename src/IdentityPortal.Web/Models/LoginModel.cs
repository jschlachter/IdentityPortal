namespace IdentityPortal.Models;

public class LoginModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public bool RememberLogin { get; set; }
    public string? ReturnUrl { get; set; }

    public string Button { get; set; } = null!;
}
