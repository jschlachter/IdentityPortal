namespace IdentityPortal.EntityFramework.Entities;

public class AuditableEntity : IAuditable
{
    DateTime _created;
    string _createdBy = null!;
    DateTime? _updated;
    string? _updatedBy;

    public DateTime Created => _created;

    public string CreatedBy => _createdBy;

    public DateTime? Updated => _updated;

    public string? UpdatedBy => _updatedBy;
}
