using EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF03.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // Composite primary key: a student can enroll in the same course only once.
            builder.HasKey(e => new { e.StudentId, e.CourseId });

            builder.Property(e => e.EnrollmentDate)
                .IsRequired();

            builder.Property(e => e.Progress)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired();

            builder.Property(e => e.FinalGrade)
                .HasPrecision(5, 2);

            // Progress must stay between 0 and 100.
            builder.ToTable(t => t.HasCheckConstraint("CK_Enrollments_Progress", "[Progress] >= 0 AND [Progress] <= 100"));

            builder.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Enrollment { StudentId = 1, CourseId = 2, EnrollmentDate = new DateTime(2024, 3, 1), Progress = 40, Status = EnrollmentStatus.Active, FinalGrade = null },
                new Enrollment { StudentId = 2, CourseId = 2, EnrollmentDate = new DateTime(2024, 3, 3), Progress = 75, Status = EnrollmentStatus.Active, FinalGrade = null },
                new Enrollment { StudentId = 1, CourseId = 5, EnrollmentDate = new DateTime(2024, 2, 10), Progress = 20, Status = EnrollmentStatus.Active, FinalGrade = null },
                new Enrollment { StudentId = 3, CourseId = 5, EnrollmentDate = new DateTime(2024, 2, 12), Progress = 100, Status = EnrollmentStatus.Completed, FinalGrade = 92.5m },
                new Enrollment { StudentId = 2, CourseId = 1, EnrollmentDate = new DateTime(2024, 4, 1), Progress = 10, Status = EnrollmentStatus.Active, FinalGrade = null }
            );
        }
    }
}
