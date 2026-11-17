using EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF03.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.Property(c => c.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(c => c.Level)
                .IsRequired();

            builder.Property(c => c.IsPublished)
                .IsRequired();

            builder.HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Course { Id = 1, Title = "C# Fundamentals", Description = "Learn C# from scratch.", Price = 49.99m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 1, 10), InstructorId = 1, CategoryId = 2 },
                new Course { Id = 2, Title = "ASP.NET Core Web APIs", Description = "Build REST APIs with ASP.NET Core.", Price = 129.99m, Level = CourseLevel.Intermediate, IsPublished = true, PublishedDate = new DateTime(2024, 2, 1), InstructorId = 1, CategoryId = 3 },
                new Course { Id = 3, Title = "React for Beginners", Description = "Start building user interfaces with React.", Price = 79.99m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 3, 5), InstructorId = 2, CategoryId = 4 },
                new Course { Id = 4, Title = "Advanced EF Core", Description = "A deep dive into EF Core.", Price = 149.99m, Level = CourseLevel.Advanced, IsPublished = false, PublishedDate = null, InstructorId = 1, CategoryId = 3 },
                new Course { Id = 5, Title = "SQL Server Essentials", Description = "Work with SQL Server databases.", Price = 59.99m, Level = CourseLevel.Beginner, IsPublished = true, PublishedDate = new DateTime(2024, 1, 20), InstructorId = 2, CategoryId = 5 }
            );
        }
    }
}
