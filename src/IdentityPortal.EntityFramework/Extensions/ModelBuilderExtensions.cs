using IdentityPortal.EntityFramework.Entities;
using IdentityPortal.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityPortal.EntityFramework.Extensions;

public static class ModelBuilderExtensions
{
    private static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, TableConfiguration configuration)
        where TEntity : class
    {
        return string.IsNullOrWhiteSpace(configuration.Schema) ? entityTypeBuilder.ToTable(configuration.Name) : entityTypeBuilder.ToTable(configuration.Name, configuration.Schema);
    }

    static EntityTypeBuilder<TEntity> IsAuditable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder)
        where TEntity : class, IAuditable
    {
        //
        // Properties
        entityTypeBuilder.Property(t => t.Created).IsRequired();
        entityTypeBuilder.Property(t => t.CreatedBy).HasMaxLength(256).IsRequired();
        entityTypeBuilder.Property(t => t.UpdatedBy).HasMaxLength(256);

        //
        // Columns
        entityTypeBuilder.Property(t => t.Created).HasColumnName("created");
        entityTypeBuilder.Property(t => t.CreatedBy).HasColumnName("created_by");
        entityTypeBuilder.Property(t => t.Updated).HasColumnName("updated");
        entityTypeBuilder.Property(t => t.UpdatedBy).HasColumnName("updated_by");

        return entityTypeBuilder;
    }

    public static void ConfigureIdentityPortalDbContext(this ModelBuilder modelBuilder, IdentityPortalStoreOptions storeOptions)
    {
        if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema)) modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

        modelBuilder.Entity<IdentityPortalTenant>(tenant => {
            //
            // Primary Key
            tenant.HasKey(t => t.Id);

            //
            // Properties
            tenant.Property(t => t.Name).HasMaxLength(75).IsRequired();
            tenant.Property(t => t.IsActive).IsRequired();

            //
            // Table & Columns
            tenant.ToTable(storeOptions.Tenants);
            tenant.Property(t => t.Id).HasColumnName("id");
            tenant.Property(t => t.ApiKey).HasColumnName("api_Key");
            tenant.Property(t => t.Name).HasColumnName("tenant_name");

            //
            // Relationships
            tenant.HasMany(tenant => tenant.Users).WithMany(user => user.Tenants)
                .UsingEntity<IdentityPortalTenantUser>(
                    j => j.HasOne(tu => tu.User).WithMany().HasForeignKey("user_id"),
                    j => j.HasOne(tu => tu.Tenant).WithMany().HasForeignKey("tenant_id"),
                    j => {
                        j.IsAuditable();
                        j.ToTable(storeOptions.TenantUsers);
                    }
                );

            tenant.HasIndex(t => t.Name)
                .HasDatabaseName("IX_IdenityPortalTenant_TenantName")
                .HasFilter("[active] = 1");
        });

        modelBuilder.Entity<IdentityPortalRole>(role =>
        {
            //
            // Primary Key
            role.HasKey(t => t.Id);

            //
            // Properties
            role.Property(t => t.ConcurrencyStamp).IsConcurrencyToken();
            role.Property(t => t.Name).HasMaxLength(75);

            //
            // Table & Columns
            role.ToTable(storeOptions.Roles);
            role.Property(t => t.Id).HasColumnName("id");
            role.Property(t => t.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            role.Property(t => t.Name).HasColumnName("role_name");

            //
            // Relationships
        });

        modelBuilder.Entity<IdentityPortalUser>(user =>
        {
            //
            // Primary Key
            user.HasKey(t => t.Id);

            //
            // Properties
            user.Property(t => t.AccessFailedCount).IsRequired();
            user.Property(t => t.UserName).HasMaxLength(256);
            user.Property(t => t.Email).HasMaxLength(256);
            user.Property(t => t.EmailConfirmed).IsRequired();
            user.Property(t => t.LockoutEnabled).IsRequired();
            user.Property(t => t.PasswordHash);
            user.Property(t => t.PhoneNumberConfirmed).IsRequired();
            user.Property(t => t.ConcurrencyStamp).IsConcurrencyToken();

            user.IsAuditable();

            //
            // Table & Columns
            user.ToTable(storeOptions.Users);

            user.Property(t => t.Id).HasColumnName("id");
            user.Property(t => t.AccessFailedCount).HasColumnName("access_failed_count");
            user.Property(t => t.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            user.Property(t => t.UserName).HasColumnName("user_name");
            user.Property(t => t.Email).HasColumnName("email");
            user.Property(t => t.EmailConfirmed).HasColumnName("email_confirmed");
            user.Property(t => t.LockoutEnabled).HasColumnName("lockout_enabled");
            user.Property(t => t.LockoutEnd).HasColumnName("lockout_end");
            user.Property(t => t.PasswordHash).HasColumnName("password_hash");
            user.Property(t => t.PhoneNumber).HasColumnName("phone_number");
            user.Property(t => t.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            user.Property(t => t.TwoFactorEnabled).HasColumnName("two_factor_enabled");

            user.HasIndex(t => t.UserName).HasDatabaseName("IX_IdentityPortalUser_UserName");

            //
            // Relationships
        });

        modelBuilder.Entity<IdentityPortalTenant>();
    }
}
