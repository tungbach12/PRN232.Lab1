namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Query request parameters for searching, sorting, and paginating student enrollments.
/// </summary>
public class EnrollmentQueryRequest
{
    /// <summary>
    /// Optional search term to filter enrollments by status.
    /// </summary>
    /// <example>Active</example>
    public string? Search { get; set; }

    /// <summary>
    /// Sorting string format (e.g. 'status', '-status').
    /// </summary>
    /// <example>-status</example>
    public string? Sort { get; set; }

    /// <summary>
    /// The page number (1-based index) to retrieve.
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of enrollments per page to return.
    /// </summary>
    /// <example>10</example>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Comma-separated list of properties to include in the response (Data Shaping).
    /// </summary>
    /// <example>enrollmentId,status,enrollDate</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related tables/entities to expand (e.g. 'student', 'course').
    /// </summary>
    /// <example>student,course</example>
    public string? Expand { get; set; }
}
