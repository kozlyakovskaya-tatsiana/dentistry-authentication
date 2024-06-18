using Application;
using FluentValidation.AspNetCore;
using Persistence;
using WebApi.Extensions;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();

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
