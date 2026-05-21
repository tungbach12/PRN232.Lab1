namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// Response model containing subject details.
/// </summary>
public class SubjectResponse
{
    /// <summary>
    /// Unique identifier for the subject.
    /// </summary>
    public int SubjectId { get; set; }

    /// <summary>
    /// Unique code for the subject.
    /// </summary>
    public string SubjectCode { get; set; } = string.Empty;

    /// <summary>
    /// Name of the subject.
    /// </summary>
    public string SubjectName { get; set; } = string.Empty;

    /// <summary>
    /// Number of credits for the subject.
    /// </summary>
    public int Credit { get; set; }

    /// <summary>
    /// List of courses for the subject if expanded.
    /// </summary>
    public List<CourseResponse>? Courses { get; set; }
}
