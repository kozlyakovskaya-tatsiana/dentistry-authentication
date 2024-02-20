using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddDatabaseConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DentistryAuthenticationContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
