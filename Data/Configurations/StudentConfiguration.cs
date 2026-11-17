using EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF03.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.DateOfBirth)
                .IsRequired();

            builder.HasData(
                new Student { Id = 1, FullName = "Youssef Nasser", Email = "youssef.nasser@example.com", DateOfBirth = new DateTime(2000, 5, 14) },
                new Student { Id = 2, FullName = "Mona Fathy", Email = "mona.fathy@example.com", DateOfBirth = new DateTime(1999, 11, 2) },
                new Student { Id = 3, FullName = "Laila Hassan", Email = "laila.hassan@example.com", DateOfBirth = new DateTime(2001, 8, 23) }
            );
        }
    }
}
