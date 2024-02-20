using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence
{
    public class DentistryAuthenticationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> Roles => Set<UserRole>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DentistryAuthenticationContext(DbContextOptions<DentistryAuthenticationContext>  options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}
