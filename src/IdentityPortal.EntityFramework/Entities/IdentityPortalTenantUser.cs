namespace IdentityPortal.EntityFramework.Entities;

public class IdentityPortalTenantUser : AuditableEntity
{
    public IdentityPortalUser User { get; set; }
    public IdentityPortalTenant Tenant { get; set; }
}
