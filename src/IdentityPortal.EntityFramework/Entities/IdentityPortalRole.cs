namespace IdentityPortal.EntityFramework.Entities;

public class IdentityPortalRole
{
    public IdentityPortalRole()
    {
        Users = new List<IdentityPortalUser>();
    }

    public int Id { get; set; }
    public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }

    public IdentityPortalTenant Tenant { get; set; }

    public List<IdentityPortalUser> Users { get; set; }
}
