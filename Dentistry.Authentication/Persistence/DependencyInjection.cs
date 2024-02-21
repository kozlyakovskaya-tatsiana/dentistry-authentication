using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DentistryAuthenticationContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<DentistryAuthenticationSeeder>();

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserRolesRepository, UserRoleRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        }
    }
}
