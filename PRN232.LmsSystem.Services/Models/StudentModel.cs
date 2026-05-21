namespace PRN232.LmsSystem.Services.Models;

public class StudentModel
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public IReadOnlyList<EnrollmentModel>? Enrollments { get; set; }
}
