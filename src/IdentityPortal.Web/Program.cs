#pragma warning disable SA1200 // UsingDirectivesMustBePlacedWithinNamespace

using System.IdentityModel.Tokens.Jwt;
using IdentityPortal;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/host-.log",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 1_000_000,
        rollOnFileSizeLimit: true,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1)
    )
    .CreateBootstrapLogger();

Log.Information("Starting host");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //
    // Add services to the container.
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

    builder.Services.AddIdentityServer((options) => {
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients);

    // ...
    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    builder.Services
    .AddAuthorization((options) => {
        // options.FallbackPolicy = new AuthorizationPolicyBuilder()
        //     .RequireAuthenticatedUser()
        //     .Build();
    })
    .AddAuthentication((options) => {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", (options) => {
        options.Authority = "";

        options.ClientId = "portal";
        options.ClientSecret = "";
        options.ResponseType = "code";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");

        options.SaveTokens = true;
    });

    builder.Host.UseSerilog((ctx, config) =>
    {
        config
            .WriteTo.File("logs/application-.log",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1)
            )
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(ctx.Configuration);

        if (ctx.HostingEnvironment.IsDevelopment())
        {
            config.WriteTo.Console();
        }
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseIdentityServer();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unknown error occured on startup");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
