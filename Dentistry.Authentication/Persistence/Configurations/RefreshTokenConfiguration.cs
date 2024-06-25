using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                .HasKey(rt => rt.Id);
            builder
                .Property(rt => rt.Token)
                .IsRequired();
            builder
                .Property(rt => rt.ExpireDateTime)
                .IsRequired();
        }
    }
}
