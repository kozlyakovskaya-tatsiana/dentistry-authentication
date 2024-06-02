using Domain.Consts;
using Domain.Entities;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Role = Domain.Entities.Role;

namespace Persistence
{
    public class DentistryAuthenticationSeeder
    {
        private readonly DentistryAuthenticationContext _dentistryAuthenticationContext;
        private readonly IPasswordHasher _passwordHasher;

        public DentistryAuthenticationSeeder(
            DentistryAuthenticationContext dentistryAuthenticationContext, IPasswordHasher passwordHasher)
        {
            _dentistryAuthenticationContext = dentistryAuthenticationContext;
            _passwordHasher = passwordHasher;
        }

        public async Task SeedAsync()
        {
            await _dentistryAuthenticationContext.Database.MigrateAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoles.Admin))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new Role { Name = UserRoles.Admin });
            }

            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoles.Doctor))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new Role { Name = UserRoles.Doctor });
            }

            if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == UserRoles.Patient))
            {
                await _dentistryAuthenticationContext.Roles.AddAsync(new Role { Name = UserRoles.Patient });
            }

            await _dentistryAuthenticationContext.SaveChangesAsync();
        }

        private async Task SeedUsersAsync()
        {
            if (!await _dentistryAuthenticationContext.Users.AnyAsync())
            {
                var roles = await _dentistryAuthenticationContext.Roles.ToArrayAsync();
                var adminRole = roles.First(role => role.Name == UserRoles.Admin);
                var doctorRole = roles.First(role => role.Name == UserRoles.Doctor);
                var patientRole = roles.First(role => role.Name == UserRoles.Patient);
                var users = new User[]
                {
                    new("111111", "admin@test.com", "admin#", new[] { adminRole }, _passwordHasher),
                    new("222222", "doctor@test.com", "doctor#", new[] { doctorRole }, _passwordHasher),
                    new("333333", "patient@test.com", "patient#", new[] { patientRole }, _passwordHasher),
                };

                _dentistryAuthenticationContext.Users.AddRange(users);
                await _dentistryAuthenticationContext.SaveChangesAsync();
            }
        }
    }
}
