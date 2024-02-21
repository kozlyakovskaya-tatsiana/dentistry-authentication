using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DentistryAuthenticationSeeder
    {
        private readonly DentistryAuthenticationContext _dentistryAuthenticationContext;

        public DentistryAuthenticationSeeder(DentistryAuthenticationContext dentistryAuthenticationContext)
        {
            _dentistryAuthenticationContext = dentistryAuthenticationContext;
        }

        public async Task SeedAsync()
        {
            await _dentistryAuthenticationContext.Database.MigrateAsync();
            await SeedRolesAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoles.Admin))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new UserRole { Name = UserRoles.Admin });
            }
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoles.Doctor))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new UserRole { Name = UserRoles.Doctor });
            }
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoles.Patient))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new UserRole { Name = UserRoles.Patient });
            }

            await _dentistryAuthenticationContext.SaveChangesAsync();
        }
    }
}
