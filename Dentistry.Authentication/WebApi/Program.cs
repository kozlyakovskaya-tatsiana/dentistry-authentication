using Application;
using FluentValidation.AspNetCore;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Persistence;
using WebApi.Extensions;
using WebApi.Middleware;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init main.");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerConfiguration();

    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();

    builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
    builder.Services.AddDatabaseServices(builder.Configuration);
    builder.Services.AddApplicationServices();

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();

    builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        SeedDataAsync().GetAwaiter().GetResult();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<ExceptionMiddleware>();

    app.MapControllers();

    app.Run();

    async Task SeedDataAsync()
    {
        using var scope = app.Services.CreateScope();
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DentistryAuthenticationSeeder>();
        await dbInitializer.SeedAsync();
    }
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

