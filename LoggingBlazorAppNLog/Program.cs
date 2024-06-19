using LoggingBlazorAppNLog.Components;
using NLog;
using NLog.Web;
using System.Linq.Expressions;

//se crea un logger con NLog
var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
//mando estos logs para que escriba en los distintos archivos.
logger.Debug("init main");
logger.Warn("Method invoked.");
logger.Error("ERROR invoked.");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    //Agregamos NLog a las dependencias
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();

}
catch
{
    //se loguea el error
    logger.Error("Program has stopped because there was an exception");
    throw;
}
finally
{
    //se cierra NLog
    NLog.LogManager.Shutdown(); 
}
