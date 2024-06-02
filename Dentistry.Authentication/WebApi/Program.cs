using Application;
using FluentValidation.AspNetCore;
using Persistence;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    SeedDataAsync().GetAwaiter().GetResult();
}

app.UseHttpsRedirection();

app.UseApplicationMiddlewares();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedDataAsync()
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DentistryAuthenticationSeeder>();
    await dbInitializer.SeedAsync();
}
