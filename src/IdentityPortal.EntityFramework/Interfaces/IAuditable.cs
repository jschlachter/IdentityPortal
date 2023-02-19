namespace IdentityPortal.EntityFramework.Entities;

public interface IAuditable
{
    DateTime Created { get; }
    string CreatedBy { get; }
    DateTime? Updated { get; }
    string? UpdatedBy { get; }
}
