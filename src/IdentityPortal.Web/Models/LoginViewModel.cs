using System.Text;


namespace IdentityPortal.Models;

public class LoginViewModel
{
    public LoginViewModel() : this("/") { }

    public LoginViewModel(string? returnUrl)
    {
        Input = new LoginModel{ ReturnUrl = returnUrl };
    }

    public LoginModel Input { get; set; }

    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;
    public IEnumerable<LoginViewModel.ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    public IEnumerable<LoginViewModel.ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => x.DisplayName.IsPresent());

    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    public string? ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;

    public class ExternalProvider
    {
        public string DisplayName { get; set; } = null!;
        public string AuthenticationScheme { get; set; } = null!;
    }
}
