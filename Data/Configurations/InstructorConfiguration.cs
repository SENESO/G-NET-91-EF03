using EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF03.Data.Configurations
{
    public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Bio)
                .HasMaxLength(500);

            builder.Property(i => i.JoinedDate)
                .IsRequired();

            builder.HasData(
                new Instructor { Id = 1, FullName = "Karim Adel", Email = "karim.adel@example.com", Bio = ".NET developer and trainer.", JoinedDate = new DateTime(2022, 3, 1) },
                new Instructor { Id = 2, FullName = "Nourhan Samy", Email = "nourhan.samy@example.com", Bio = "Frontend developer and UI specialist.", JoinedDate = new DateTime(2023, 6, 15) }
            );
        }
    }
}
