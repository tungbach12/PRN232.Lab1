namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Query request parameters for searching, sorting, and paginating courses.
/// </summary>
public class CourseQueryRequest
{
    /// <summary>
    /// Optional search term to filter courses by CourseName.
    /// </summary>
    /// <example>Course 1</example>
    public string? Search { get; set; }

    /// <summary>
    /// Sorting string format (e.g. 'coursename', '-semesterid', 'subjectid').
    /// </summary>
    /// <example>coursename</example>
    public string? Sort { get; set; }

    /// <summary>
    /// The page number (1-based index) to retrieve.
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of courses per page to return.
    /// </summary>
    /// <example>10</example>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Comma-separated list of properties to include in the response (Data Shaping).
    /// </summary>
    /// <example>courseId,courseName</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related tables/entities to expand (e.g. 'semester,subject,enrollments').
    /// </summary>
    /// <example>semester,subject</example>
    public string? Expand { get; set; }
}
