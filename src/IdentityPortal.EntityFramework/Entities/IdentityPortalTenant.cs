namespace IdentityPortal.EntityFramework.Entities;

public class IdentityPortalTenant : AuditableEntity
{
    public IdentityPortalTenant()
    {
        Users = new List<IdentityPortalUser>();
    }

    public int Id { get; set; }
    public string? ApiKey { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }

    public List<IdentityPortalUser> Users { get; set; }

    public List<IdentityPortalRole> Roles { get; set; }
}
