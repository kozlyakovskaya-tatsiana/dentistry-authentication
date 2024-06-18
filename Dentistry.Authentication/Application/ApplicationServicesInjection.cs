using Application.Features.Roles.Queries;
using Application.Mapster;
using Application.Services;
using Application.Services.Implementation;
using Domain.Services;
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

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
