using Domain.Entities;
using Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Role = Domain.Entities.Role;

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
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoleType.Admin.Name))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new Role { Name = UserRoleType.Admin.Name });
            }
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoleType.Doctor.Name))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new Role { Name = UserRoleType.Doctor.Name });
            }
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoleType.Patient.Name))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new Role { Name = UserRoleType.Patient.Name });
            }

            await _dentistryAuthenticationContext.SaveChangesAsync();
        }
    }
}
