namespace IdentityPortal.Storage.Models;

public interface IUserStore
{
    bool ValidateCredentials(string username, string password);
    Task<IdentityPortalUser> FindById(int userId);
    Task<IdentityPortalUser> FindByUsername(string username);
}
