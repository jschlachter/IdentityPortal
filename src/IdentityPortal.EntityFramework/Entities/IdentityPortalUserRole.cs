namespace IdentityPortal.EntityFramework.Entities;

public class IdentityPortalUserRole : AuditableEntity
{
    public IdentityPortalUser User { get; set; }
    public IdentityPortalRole Role { get; set;  }
}
