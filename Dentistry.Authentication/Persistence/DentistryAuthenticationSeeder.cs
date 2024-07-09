using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

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
        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == "admin"))
        {
            await _dentistryAuthenticationContext.Roles.AddAsync(Role.Create("admin"));
        }

        if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == "doctor"))
        {
            await _dentistryAuthenticationContext.Roles.AddAsync(Role.Create("doctor"));
        }

        if (!_dentistryAuthenticationContext.Roles.Any(role => role.Name == "patient"))
        {
            await _dentistryAuthenticationContext.Roles.AddAsync(Role.Create("patient"));
        }

        await _dentistryAuthenticationContext.SaveChangesAsync();
    }

    private async Task SeedUsersAsync()
    {
        if (!await _dentistryAuthenticationContext.Users.AnyAsync())
        {
            var roles = await _dentistryAuthenticationContext.Roles.ToArrayAsync();
            var adminRole = roles.First(role => role.Name == "admin");
            var doctorRole = roles.First(role => role.Name == "doctor");
            var patientRole = roles.First(role => role.Name == "patient");

            const string password = "default_password";
            const string passwordHash = "$2a$11$l3Tfn90Fk3IrlzWjLTbYLO.TorOFClsEeUVaWTaSjaqdHH/SynGVu";

            var admin = User.Create("+375296026404", "admin@test.com", passwordHash, new[] { adminRole }, null);
            var user = User.Create("+375296026405", "doctor@test.com", passwordHash, new[] { doctorRole }, null);
            var patient = User.Create("+375296026406", "patient@test.com", passwordHash, new[] { patientRole }, null);
                
            _dentistryAuthenticationContext.Users.AddRange(admin, user, patient);

            await _dentistryAuthenticationContext.SaveChangesAsync();
        }
    }
}