using IdentityPortal.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace IdentityPortal.EntityFramework;

public static class IdentityPortalEntityFrameworkBuilderExtensions
{
    /// <summary>
    /// Add Job Context to the DI system.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityPortalDbContext(this IServiceCollection services,
        Action<IdentityPortalStoreOptions>? storeOptionsAction = null)
    {
        return services.AddIdentityPortalDbContext<IdentityPortalDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Add Configuration DbContext to the DI system.
    /// </summary>
    /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityPortalDbContext<TContext>(this IServiceCollection services,
        Action<IdentityPortalStoreOptions>? storeOptionsAction = null)
        where TContext : DbContext, IIdentityPortalDbContext
    {
        var options = new IdentityPortalStoreOptions();
        services.AddSingleton(options);
        storeOptionsAction?.Invoke(options);

        if (options.EnablePooling)
        {
            if (options.PoolSize.HasValue)
            {
                services.AddDbContextPool<TContext>(
                    dbCtxBuilder => { options.ConfigureDbContext?.Invoke(dbCtxBuilder); }, options.PoolSize.Value);
            }
            else
            {
                services.AddDbContextPool<TContext>(
                    dbCtxBuilder => { options.ConfigureDbContext?.Invoke(dbCtxBuilder); });
            }
        }
        else
        {
            services.AddDbContext<IIdentityPortalDbContext, TContext>(dbCtxBuilder =>
            {
                options.ConfigureDbContext?.Invoke(dbCtxBuilder);
            });
        }

        return services;
    }
}
