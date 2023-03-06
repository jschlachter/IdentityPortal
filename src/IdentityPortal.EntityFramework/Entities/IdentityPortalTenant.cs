namespace IdentityPortal.EntityFramework.Entities;

public class IdentityPortalTenant : AuditableEntity
{
    public IdentityPortalTenant()
    {
        Roles = new List<IdentityPortalRole>();
    }

    public int Id { get; set; }
    public string? ApiKey { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }

    public List<IdentityPortalRole> Roles { get; set; }
}
