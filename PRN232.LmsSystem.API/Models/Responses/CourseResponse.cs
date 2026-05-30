namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// Response model containing course details.
/// </summary>
public class CourseResponse
{
    /// <summary>
    /// Unique identifier for the course.
    /// </summary>
    public int CourseId { get; set; }

    /// <summary>
    /// Name of the course.
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier for the semester.
    /// </summary>
    public int SemesterId { get; set; }

    /// <summary>
    /// Semester details if expanded.
    /// </summary>
    public SemesterResponse? Semester { get; set; }

    /// <summary>
    /// Enrollments for this course if expanded.
    /// </summary>
    public List<EnrollmentResponse>? Enrollments { get; set; }
}

