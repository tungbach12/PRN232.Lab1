namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// Response model containing semester details.
/// </summary>
public class SemesterResponse
{
    /// <summary>
    /// Unique identifier for the semester.
    /// </summary>
    public int SemesterId { get; set; }

    /// <summary>
    /// Name of the semester.
    /// </summary>
    public string SemesterName { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the semester.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the semester.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// List of courses in the semester if expanded.
    /// </summary>
    public List<CourseResponse>? Courses { get; set; }
}
