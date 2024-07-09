using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                .HasKey(rt => rt.Id);

            builder
                .Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(rt => rt.ExpireDateTime)
                .IsRequired();

            builder
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .HasConstraintName("FK_User_RefreshToken");
        }
    }
}
