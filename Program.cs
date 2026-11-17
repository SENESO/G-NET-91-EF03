using EF03.Data;
using EF03.Models;
using Microsoft.EntityFrameworkCore;

namespace EF03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new OnlineLearningContext())
            {
                context.Database.Migrate();
                Console.WriteLine("Database migrations applied successfully.");

                Console.WriteLine();
                Console.WriteLine("========== Create Operations ==========");
                var instructor = AddInstructor(context, "Hany George", "hany.george@example.com",
                    "Software engineer and .NET instructor.", new DateTime(2024, 5, 12));

                AddCourse(context, "Microservices with .NET", 119.99m, CourseLevel.Intermediate, 999, 3);
                AddCourse(context, "Microservices with .NET", 119.99m, CourseLevel.Intermediate, instructor.Id, 3);

                AddSeveralStudents(context);

                EnrollStudent(context, 999, 1);
                EnrollStudent(context, 1, 999);
                EnrollStudent(context, 3, 4);
                EnrollStudent(context, 1, 2);
                EnrollStudent(context, 3, 1);

                Console.WriteLine();
                Console.WriteLine("========== Read Operations ==========");
                FindCourse(context, 2, "React for Beginners");
                FindCourse(context, 999, "Python for Everybody");
                FilterCourses(context, 50m, 150m);
                PrintSubCategories(context, 1);
                PrintCourseRoster(context, 2);

                Console.WriteLine();
                Console.WriteLine("========== Update Operations ==========");
                ChangePrice(context, 1, 44.99m);
                UpdateProgress(context, 1, 2, 100);
                UpdateProgress(context, 1, 2, 150);
                UpdateProgress(context, 999, 2, 50);
                PublishCourse(context, 4);

                Console.WriteLine();
                Console.WriteLine("========== Delete Operations ==========");
                Unenroll(context, 2, 1);
                Unenroll(context, 2, 1);
                DeleteCategory(context, 2);
                DeleteCategorySafe(context, 2);
                DeleteCourse(context, 5);
            }
        }

        // ---------- Create ----------

        static Instructor AddInstructor(OnlineLearningContext context, string fullName, string email, string bio, DateTime joinedDate)
        {
            var instructor = new Instructor
            {
                FullName = fullName,
                Email = email,
                Bio = bio,
                JoinedDate = joinedDate
            };

            context.Instructors.Add(instructor);
            context.SaveChanges();

            Console.WriteLine($"Instructor '{instructor.FullName}' was added with Id {instructor.Id}.");
            return instructor;
        }

        static Course AddCourse(OnlineLearningContext context, string title, decimal price, CourseLevel level, int instructorId, int categoryId)
        {
            var instructor = context.Instructors.Find(instructorId);
            if (instructor == null)
            {
                Console.WriteLine($"Course was not added: instructor with Id {instructorId} does not exist.");
                return null;
            }

            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Course was not added: category with Id {categoryId} does not exist.");
                return null;
            }

            var course = new Course
            {
                Title = title,
                Description = "A practical course about building microservices with .NET.",
                Price = price,
                Level = level,
                IsPublished = false,
                PublishedDate = null,
                InstructorId = instructorId,
                CategoryId = categoryId
            };

            context.Courses.Add(course);
            context.SaveChanges();

            Console.WriteLine($"Course '{course.Title}' was added with Id {course.Id}.");
            return course;
        }

        static List<Student> AddSeveralStudents(OnlineLearningContext context)
        {
            var students = new List<Student>
            {
                new Student { FullName = "Ali Samy", Email = "ali.samy@example.com", DateOfBirth = new DateTime(2002, 1, 19) },
                new Student { FullName = "Dina Magdy", Email = "dina.magdy@example.com", DateOfBirth = new DateTime(2000, 7, 30) },
                new Student { FullName = "Tamer Adel", Email = "tamer.adel@example.com", DateOfBirth = new DateTime(1998, 12, 5) }
            };

            context.Students.AddRange(students);
            context.SaveChanges();

            foreach (var student in students)
                Console.WriteLine($"Student '{student.FullName}' was added with Id {student.Id}.");

            return students;
        }

        static bool EnrollStudent(OnlineLearningContext context, int studentId, int courseId)
        {
            var student = context.Students.Find(studentId);
            if (student == null)
            {
                Console.WriteLine($"Enrollment refused: student with Id {studentId} does not exist.");
                return false;
            }

            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Enrollment refused: course with Id {courseId} does not exist.");
                return false;
            }

            if (!course.IsPublished)
            {
                Console.WriteLine($"Enrollment refused: course '{course.Title}' is not published yet.");
                return false;
            }

            var existing = context.Enrollments.Find(studentId, courseId);
            if (existing != null)
            {
                Console.WriteLine($"Enrollment refused: '{student.FullName}' is already enrolled in '{course.Title}'.");
                return false;
            }

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Progress = 0,
                Status = EnrollmentStatus.Active,
                FinalGrade = null
            };

            context.Enrollments.Add(enrollment);
            context.SaveChanges();

            Console.WriteLine($"'{student.FullName}' was enrolled in '{course.Title}'.");
            return true;
        }

        // ---------- Read ----------

        static void FindCourse(OnlineLearningContext context, int id, string title)
        {
            var courseById = context.Courses.Find(id);
            if (courseById == null)
                Console.WriteLine($"Course with Id {id}: not found.");
            else
                Console.WriteLine($"Course found by Id: {courseById.Title} - {courseById.Level} - {courseById.Price}");

            var courseByTitle = context.Courses.FirstOrDefault(c => c.Title == title);
            if (courseByTitle == null)
                Console.WriteLine($"Course with title '{title}': not found.");
            else
                Console.WriteLine($"Course found by title: {courseByTitle.Title} - {courseByTitle.Level} - {courseByTitle.Price}");
        }

        static void FilterCourses(OnlineLearningContext context, decimal minPrice, decimal maxPrice)
        {
            Console.WriteLine($"Courses priced between {minPrice} and {maxPrice}:");
            var courses = context.Courses
                .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
                .OrderByDescending(c => c.Price)
                .Select(c => new { c.Title, c.Level, c.Price })
                .ToList();

            foreach (var course in courses)
                Console.WriteLine($"{course.Title} - {course.Level} - {course.Price}");
        }

        static void PrintSubCategories(OnlineLearningContext context, int categoryId)
        {
            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category with Id {categoryId} was not found.");
                return;
            }

            Console.WriteLine($"Direct sub-categories of '{category.Name}':");
            var subCategories = context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .ToList();
            foreach (var sub in subCategories)
                Console.WriteLine($"- {sub.Name}");

            Console.WriteLine("Top-level categories:");
            var topLevel = context.Categories
                .Where(c => c.ParentCategoryId == null)
                .ToList();
            foreach (var top in topLevel)
                Console.WriteLine($"- {top.Name}");
        }

        static void PrintCourseRoster(OnlineLearningContext context, int courseId)
        {
            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course with Id {courseId} was not found.");
                return;
            }

            var studentIds = context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.StudentId)
                .ToList();

            var students = context.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToList();

            Console.WriteLine($"Course '{course.Title}' has {students.Count} enrolled students:");
            foreach (var student in students)
                Console.WriteLine($"- {student.FullName}");
        }

        // ---------- Update ----------

        static void ChangePrice(OnlineLearningContext context, int courseId, decimal newPrice)
        {
            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course with Id {courseId} was not found.");
                return;
            }

            Console.WriteLine($"Changing the price of '{course.Title}' from {course.Price} to {newPrice}.");
            course.Price = newPrice;
            context.SaveChanges();
            Console.WriteLine($"The price of '{course.Title}' is now {course.Price}.");
        }

        static void UpdateProgress(OnlineLearningContext context, int studentId, int courseId, int newProgress)
        {
            if (newProgress < 0 || newProgress > 100)
            {
                Console.WriteLine($"Progress {newProgress} was rejected: progress must be between 0 and 100.");
                return;
            }

            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine($"Enrollment of student {studentId} in course {courseId} was not found.");
                return;
            }

            enrollment.Progress = newProgress;
            if (newProgress == 100)
                enrollment.Status = EnrollmentStatus.Completed;

            context.SaveChanges();
            Console.WriteLine($"Progress of student {studentId} in course {courseId} is now {enrollment.Progress} (Status: {enrollment.Status}).");
        }

        static void PublishCourse(OnlineLearningContext context, int courseId)
        {
            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course with Id {courseId} was not found.");
                return;
            }

            if (course.IsPublished)
            {
                Console.WriteLine($"Course '{course.Title}' is already published.");
                return;
            }

            course.IsPublished = true;
            course.PublishedDate = DateTime.Now;
            context.SaveChanges();
            Console.WriteLine($"Course '{course.Title}' was published on {course.PublishedDate:yyyy-MM-dd}.");
        }

        // ---------- Delete ----------

        static void Unenroll(OnlineLearningContext context, int studentId, int courseId)
        {
            var enrollment = context.Enrollments.Find(studentId, courseId);
            if (enrollment == null)
            {
                Console.WriteLine($"Enrollment of student {studentId} in course {courseId} was not found.");
                return;
            }

            context.Enrollments.Remove(enrollment);
            context.SaveChanges();
            Console.WriteLine($"Student {studentId} was unenrolled from course {courseId}.");
        }

        static void DeleteCategory(OnlineLearningContext context, int categoryId)
        {
            // A separate context is used here so that no sub-categories are tracked:
            // the delete is sent to the database, which rejects it because the
            // category still has sub-categories.
            using (var deleteContext = new OnlineLearningContext())
            {
                var category = deleteContext.Categories.Find(categoryId);
                if (category == null)
                {
                    Console.WriteLine($"Category with Id {categoryId} was not found.");
                    return;
                }

                try
                {
                    deleteContext.Categories.Remove(category);
                    deleteContext.SaveChanges();
                    Console.WriteLine($"Category '{category.Name}' was deleted.");
                }
                catch (DbUpdateException ex)
                {
                    string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    Console.WriteLine($"Category '{category.Name}' could not be deleted. Database error: {message}");
                }
            }
        }

        static void DeleteCategorySafe(OnlineLearningContext context, int categoryId)
        {
            var category = context.Categories.Find(categoryId);
            if (category == null)
            {
                Console.WriteLine($"Category with Id {categoryId} was not found.");
                return;
            }

            bool hasSubCategories = context.Categories.Any(c => c.ParentCategoryId == categoryId);
            if (hasSubCategories)
            {
                Console.WriteLine($"Category '{category.Name}' was not deleted: it still has sub-categories.");
                return;
            }

            context.Categories.Remove(category);
            context.SaveChanges();
            Console.WriteLine($"Category '{category.Name}' was deleted.");
        }

        static void DeleteCourse(OnlineLearningContext context, int courseId)
        {
            var course = context.Courses.Find(courseId);
            if (course == null)
            {
                Console.WriteLine($"Course with Id {courseId} was not found.");
                return;
            }

            int enrollmentsBefore = context.Enrollments.Count(e => e.CourseId == courseId);

            context.Courses.Remove(course);
            // The enrollments of this course are removed automatically because the
            // Enrollment -> Course relationship is configured with Cascade delete,
            // so the database deletes the dependent enrollment rows together with the course.
            context.SaveChanges();

            int enrollmentsAfter = context.Enrollments.Count(e => e.CourseId == courseId);
            Console.WriteLine($"Course '{course.Title}' was deleted. Enrollments before: {enrollmentsBefore}, after: {enrollmentsAfter}.");
        }
    }
}
