using EF03.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF03.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            // Self-referencing relationship: a category has one parent category
            // and can have many sub-categories. Restrict prevents deleting a
            // parent category that still has sub-categories.
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Category { Id = 1, Name = "Programming", Description = "All programming topics", ParentCategoryId = null },
                new Category { Id = 2, Name = "Backend", Description = "Server-side development", ParentCategoryId = 1 },
                new Category { Id = 3, Name = "ASP.NET Core", Description = "ASP.NET Core courses", ParentCategoryId = 2 },
                new Category { Id = 4, Name = "Frontend", Description = "Client-side development", ParentCategoryId = 1 },
                new Category { Id = 5, Name = "Databases", Description = "Database courses", ParentCategoryId = 1 }
            );
        }
    }
}
