namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Request model for creating or updating a student.
/// </summary>
public class StudentRequest
{
    /// <summary>
    /// The full name of the student.
    /// </summary>
    /// <example>John Doe</example>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The unique email address of the student.
    /// </summary>
    /// <example>johndoe@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The date of birth of the student.
    /// </summary>
    /// <example>2000-01-01</example>
    public DateTime DateOfBirth { get; set; }
}
