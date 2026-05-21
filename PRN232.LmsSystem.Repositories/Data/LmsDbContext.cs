using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Data;

public class LmsDbContext : DbContext
{
    public LmsDbContext(DbContextOptions<LmsDbContext> options) : base(options)
    {
    }

    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Semester>(entity =>
        {
            entity.Property(e => e.SemesterName).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.Property(e => e.CourseName).HasMaxLength(100);
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.Property(e => e.SubjectCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SubjectName).HasMaxLength(100);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Semester)
            .WithMany(s => s.Courses)
            .HasForeignKey(c => c.SemesterId);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Subject)
            .WithMany(s => s.Courses)
            .HasForeignKey(c => c.SubjectId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);

        var semesters = Enumerable.Range(1, 5)
            .Select(i => new Semester
            {
                SemesterId = i,
                SemesterName = $"Semester {i}",
                StartDate = new DateTime(2024, 1, 1).AddMonths((i - 1) * 4),
                EndDate = new DateTime(2024, 4, 30).AddMonths((i - 1) * 4)
            })
            .ToArray();

        var subjects = Enumerable.Range(1, 10)
            .Select(i => new Subject
            {
                SubjectId = i,
                SubjectCode = $"SUBJ{i:000}",
                SubjectName = $"Subject {i}",
                Credit = 2 + (i % 3)
            })
            .ToArray();

        var courses = Enumerable.Range(1, 20)
            .Select(i => new Course
            {
                CourseId = i,
                CourseName = $"Course {i}",
                SemesterId = ((i - 1) % 5) + 1,
                SubjectId = ((i - 1) % 10) + 1
            })
            .ToArray();

        var students = Enumerable.Range(1, 50)
            .Select(i => new Student
            {
                StudentId = i,
                FullName = $"Student {i}",
                Email = $"student{i}@lms.local",
                DateOfBirth = new DateTime(2000, 1, 1).AddDays(i * 7)
            })
            .ToArray();

        var statuses = new[] { "Active", "Completed", "Dropped" };
        var enrollments = Enumerable.Range(1, 500)
            .Select(i => new Enrollment
            {
                EnrollmentId = i,
                StudentId = ((i - 1) % 50) + 1,
                CourseId = ((i - 1) % 20) + 1,
                EnrollDate = new DateTime(2024, 1, 15).AddDays(i),
                Status = statuses[i % statuses.Length]
            })
            .ToArray();

        modelBuilder.Entity<Semester>().HasData(semesters);
        modelBuilder.Entity<Subject>().HasData(subjects);
        modelBuilder.Entity<Course>().HasData(courses);
        modelBuilder.Entity<Student>().HasData(students);
        modelBuilder.Entity<Enrollment>().HasData(enrollments);
    }
}
