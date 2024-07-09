using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DentistryAuthenticationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DentistryAuthenticationContext(DbContextOptions<DentistryAuthenticationContext>  options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DentistryAuthenticationContext).Assembly);
    }
}