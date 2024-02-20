using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(ur => ur.Id);
            builder
                .Property(ur => ur.Name)
                .IsRequired();
            builder
                .HasMany(ur => ur.Users)
                .WithMany(u => u.UserRoles);
        }
    }
}
