using IdentityPortal.EntityFramework.Entities;
using IdentityPortal.EntityFramework.Extensions;
using IdentityPortal.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace IdentityPortal.EntityFramework;

public class IdentityPortalDbContext : DbContext, IIdentityPortalDbContext
{
    public IdentityPortalDbContext(DbContextOptions<IdentityPortalDbContext> options) : base(options)
    {

    }

    public IdentityPortalDbContext(DbContextOptions<IdentityPortalDbContext> options, IdentityPortalStoreOptions storeOptions)
        : base(options)
    {
        StoreOptions = storeOptions;
    }

    /// <summary>
    /// The store options.
    /// </summary>`
    public IdentityPortalStoreOptions StoreOptions { get; private set; } = null!;

    public DbSet<IdentityPortalUser> Users => Set<IdentityPortalUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (StoreOptions is null) {
            StoreOptions = this.GetService<IdentityPortalStoreOptions>();

            if (StoreOptions is null) {
                throw new ArgumentNullException(nameof(IdentityPortalStoreOptions), "IdentityPortalStoreOptions must be configured in the DI system.");
            }
        }

        modelBuilder.ConfigureIdentityPortalDbContext(StoreOptions);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.Options.IsFrozen)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(new EventId[] { RelationalEventId.MultipleCollectionIncludeWarning }));
        }

    }
}
