using Application.Features.UsersManagement.Queries;
using Application.Mapster;
using Application.Services;
using Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationServicesInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllRolesQuery).Assembly));

            services.RegisterMapsterConfiguration();

            services.AddValidatorsFromAssembly(typeof(ApplicationServicesInjection).Assembly);

            services.AddScoped<IPasswordHashService, PasswordHashService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
