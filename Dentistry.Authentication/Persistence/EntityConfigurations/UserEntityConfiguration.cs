using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    // cancellation tokes
    public void Configure(EntityTypeBuilder<User> builder)
    {
        const string phoneNumberRegex = "^\\+375(25|29|33|44)\\d{7}$";
        const string emailRegex = "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$";
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(u => u.PasswordHash)
            .IsRequired();

        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.PhoneNumber).IsUnique();
        builder.HasCheckConstraint("CK_User_PhoneNumber", $"\"PhoneNumber\" ~* '{phoneNumberRegex}'");
        builder.HasCheckConstraint("CK_User_Email", $"\"Email\" ~* '{emailRegex}'");

        builder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users);
    }
}