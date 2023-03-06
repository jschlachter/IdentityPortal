using Microsoft.EntityFrameworkCore;

namespace IdentityPortal.EntityFramework.Options;

public class IdentityPortalStoreOptions
{
    public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

    /// <summary>
    /// Gets or sets the default schema.
    /// </summary>
    /// <value>
    /// The default schema.
    /// </value>
    public string? DefaultSchema { get; set; } = "idp";

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
    public TableConfiguration Users = new ("users");

    public TableConfiguration Roles = new("roles");

    public TableConfiguration Tenants = new("tenants");

    public TableConfiguration UserRoles = new("user_roles");
}
