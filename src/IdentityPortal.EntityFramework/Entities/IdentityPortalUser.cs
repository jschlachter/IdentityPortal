namespace IdentityPortal.EntityFramework.Entities;

public class IdentityPortalUser : AuditableEntity
{
    public IdentityPortalUser()
    {
        IsActive = true;
        Roles = new List<IdentityPortalRole>();
    }

    public int Id { get; set; }
    public int AccessFailedCount { get; set; }
    public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset LockoutEnd { get; set; }
    public string? UserName { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public bool IsActive { get; set; }
    public List<IdentityPortalRole> Roles { get; set; }
}
