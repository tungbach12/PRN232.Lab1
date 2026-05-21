namespace PRN232.LmsSystem.Repositories.Entities;

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public Semester? Semester { get; set; }
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
