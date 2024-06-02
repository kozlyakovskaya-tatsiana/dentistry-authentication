using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class DatabaseServicesInjection
    {
        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DentistryAuthenticationContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DentistryAuthenticationSeeder>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        }
    }
}
