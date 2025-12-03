using HRSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRSystem.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(x => x.PersonalNumber)
                   .IsRequired()
                   .HasMaxLength(11)
                   .IsUnicode(false);

            builder.Property(x => x.FirstName)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.Property(x => x.LastName)
                   .IsRequired()
                   .HasMaxLength(100)
                   .IsUnicode(true);

            builder.Property(x => x.Email)
                   .HasMaxLength(150)
                   .IsUnicode(true);

            builder.HasOne(x => x.Position)
                   .WithMany(x => x.Employees)
                   .HasForeignKey(x => x.PositionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CreatedByUser)
                   .WithMany(x => x.CreatedEmployees)
                   .HasForeignKey(x => x.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedByUser)
                   .WithMany(x => x.UpdatedEmployees)
                   .HasForeignKey(x => x.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
