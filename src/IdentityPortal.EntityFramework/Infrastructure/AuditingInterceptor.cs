using IdentityPortal.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IdentityPortal.EntityFramework;

public class AuditingInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        eventData.Context.ChangeTracker.DetectChanges();

        var entities = eventData.Context.ChangeTracker.Entries()
            .Where(t => t.Entity is IAuditable && (
                t.State == EntityState.Deleted
                || t.State == EntityState.Added
                || t.State == EntityState.Modified
            ));


        if (entities.Any()) {

            var timestamp = DateTimeOffset.UtcNow;

            foreach(var entry in entities)
            {
                switch(entry.State) {
                    case EntityState.Added:
                        entry.CurrentValues[nameof(IAuditable.Created)] = timestamp;
                        entry.CurrentValues[nameof(IAuditable.CreatedBy)] = "system";
                        entry.CurrentValues[nameof(IAuditable.Updated)] = timestamp;
                        entry.CurrentValues[nameof(IAuditable.UpdatedBy)] = "system";
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues[nameof(IAuditable.Updated)] = timestamp;
                        entry.CurrentValues[nameof(IAuditable.UpdatedBy)] = "system";
                        break;
                }
            }
        }

        return ValueTask.FromResult(result);
    }
}
