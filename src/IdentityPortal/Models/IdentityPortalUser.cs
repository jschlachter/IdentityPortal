namespace IdentityPortal.Storage.Models;

public class IdentityPortalUser
{
    public IdentityPortalUser ()
    {

    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
