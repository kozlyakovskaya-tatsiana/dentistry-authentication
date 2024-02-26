using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(ur => ur.Id);
            builder
                .Property(ur => ur.Name)
                .IsRequired();
            builder
                .HasMany(ur => ur.Users)
                .WithMany(u => u.Roles);
        }
    }
}
