namespace PRN232.LmsSystem.Services.Models;

public class CourseModel
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int SemesterId { get; set; }
    public SemesterModel? Semester { get; set; }
    public List<EnrollmentModel>? Enrollments { get; set; }
}

