using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ef03.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[] { 1, "All programming topics", "Programming", null });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Bio", "Email", "FullName", "JoinedDate" },
                values: new object[,]
                {
                    { 1, ".NET developer and trainer.", "karim.adel@example.com", "Karim Adel", new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Frontend developer and UI specialist.", "nourhan.samy@example.com", "Nourhan Samy", new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "Email", "FullName" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "youssef.nasser@example.com", "Youssef Nasser" },
                    { 2, new DateTime(1999, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "mona.fathy@example.com", "Mona Fathy" },
                    { 3, new DateTime(2001, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "laila.hassan@example.com", "Laila Hassan" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name", "ParentCategoryId" },
                values: new object[,]
                {
                    { 2, "Server-side development", "Backend", 1 },
                    { 4, "Client-side development", "Frontend", 1 },
                    { 5, "Database courses", "Databases", 1 },
                    { 3, "ASP.NET Core courses", "ASP.NET Core", 2 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CategoryId", "Description", "InstructorId", "IsPublished", "Level", "Price", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, 2, "Learn C# from scratch.", 1, true, 0, 49.99m, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# Fundamentals" },
                    { 3, 4, "Start building user interfaces with React.", 2, true, 0, 79.99m, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "React for Beginners" },
                    { 5, 5, "Work with SQL Server databases.", 2, true, 0, 59.99m, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "SQL Server Essentials" },
                    { 2, 3, "Build REST APIs with ASP.NET Core.", 1, true, 1, 129.99m, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core Web APIs" },
                    { 4, 3, "A deep dive into EF Core.", 1, false, 2, 149.99m, null, "Advanced EF Core" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "CourseId", "StudentId", "EnrollmentDate", "FinalGrade", "Progress", "Status" },
                values: new object[,]
                {
                    { 5, 1, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 20, 0 },
                    { 1, 2, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 10, 0 },
                    { 5, 3, new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 92.5m, 100, 1 },
                    { 2, 1, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 40, 0 },
                    { 2, 2, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 75, 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumns: new[] { "CourseId", "StudentId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
