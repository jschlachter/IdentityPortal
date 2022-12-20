#pragma warning disable SA1200 // UsingDirectivesMustBePlacedWithinNamespace

using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/host-.log",
        LogEventLevel.Debug,
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

    builder.Host.UseSerilog((ctx, config) =>
    {
        config
            .MinimumLevel.Is(ctx.HostingEnvironment.IsProduction() ? LogEventLevel.Warning : LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .WriteTo.File("logs/application-.log",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1)
            );

        if (ctx.HostingEnvironment.IsDevelopment())
        {
            config.WriteTo.Console(
                restrictedToMinimumLevel: LogEventLevel.Information);
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

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapGet("/", () => "Hello World!");

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
