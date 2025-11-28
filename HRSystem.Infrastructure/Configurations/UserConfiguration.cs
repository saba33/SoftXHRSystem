using HRSystem.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.PersonalNumber)
                   .HasMaxLength(11);

            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.PasswordSalt).IsRequired();
        }
    }
}
