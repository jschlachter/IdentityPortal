using Microsoft.EntityFrameworkCore;

namespace IdentityPortal.EntityFramework.Options;

public class IdentityPortalStoreOptions
{
    /// <summary>
    /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
    /// </summary>
    /// <value>
    /// The configure database context.
    /// </value>
    public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

    /// <summary>
    /// Gets or sets the default schema.
    /// </summary>
    /// <value>
    /// The default schema.
    /// </value>
    public string? DefaultSchema { get; set; } = "dbo";

    /// <summary>
    /// Gets or set if EF DbContext pooling is enabled.
    /// </summary>
    public bool EnablePooling { get; set; }

    /// <summary>
    /// Gets or set the pool size to use when DbContext pooling is enabled. If not set, the EF default is used.
    /// </summary>
    public int? PoolSize { get; set; }

    //
    // TODO: Add TableConfiguration values
    public TableConfiguration Users = new ("IdentityPortalUsers");

    public TableConfiguration Roles = new("IdentityPortalRole");

    public TableConfiguration Tenants = new("IdentityPortalTenants");

    public TableConfiguration TenantUsers = new("IdentityPortalTenantUsers");
}
