namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// Response model containing student details.
/// </summary>
public class StudentResponse
{
    /// <summary>
    /// Unique identifier for the student.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// The full name of the student.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the student.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The date of birth of the student.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// List of student's enrollments if expanded.
    /// </summary>
    public IReadOnlyList<EnrollmentResponse>? Enrollments { get; set; }
}
