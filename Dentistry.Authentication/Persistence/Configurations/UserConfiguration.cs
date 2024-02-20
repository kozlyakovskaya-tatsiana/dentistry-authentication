﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.PhoneNumber)
                .IsRequired();
            builder
                .Property(u => u.PasswordHash)
                .IsRequired();

            builder
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User);
            builder
                .HasMany(r => r.UserRoles)
                .WithMany(ur => ur.Users);
        }
    }
}
