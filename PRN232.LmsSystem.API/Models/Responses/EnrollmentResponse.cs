namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// Response model containing enrollment details.
/// </summary>
public class EnrollmentResponse
{
    /// <summary>
    /// Unique identifier for the enrollment.
    /// </summary>
    public int EnrollmentId { get; set; }

    /// <summary>
    /// Unique identifier of the student.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Unique identifier of the course.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// The date the enrollment was created.
    /// </summary>
    public DateTime EnrollDate { get; set; }

    /// <summary>
    /// The status of the enrollment.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Student details if expanded.
    /// </summary>
    public StudentResponse? Student { get; set; }

    /// <summary>
    /// Course details if expanded.
    /// </summary>
    public CourseResponse? Course { get; set; }
}
