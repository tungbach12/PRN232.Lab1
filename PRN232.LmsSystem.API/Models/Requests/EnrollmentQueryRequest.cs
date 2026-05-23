namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Container for query request parameters designed to search, filter, sort, paginate, and dynamically shape student enrollment records.
/// </summary>
/// <remarks>
/// This class enables powerful client-driven query operations through HTTP query string parameters. 
/// It supports full data shaping (selective fields), dynamic multi-property sorting, paginated results, and dynamic eager-loading of navigation properties.
/// </remarks>
public class EnrollmentQueryRequest
{
    /// <summary>
    /// Search term to filter enrollment records. Case-insensitive partial match against:
    /// - Enrollment Status (e.g. "Active", "Completed", "Dropped")
    /// - Student's Full Name (if the student navigation entity is loaded)
    /// - Course's Name (if the course navigation entity is loaded)
    /// </summary>
    /// <example>Active</example>
    public string? Search { get; set; }

    /// <summary>
    /// Dynamic multi-property sorting expression. Comma-separated properties.
    /// Prefix a property with '-' for descending order. Default is ascending.
    /// Valid properties (Case-Insensitive): enrollmentId, studentId, courseId, enrollDate, status.
    /// Example: "-enrollDate,status"
    /// </summary>
    /// <example>-enrollDate,status</example>
    public string? Sort { get; set; }

    /// <summary>
    /// Page number (1-based index) of the paginated results to retrieve.
    /// Minimum value is 1. If less than 1 is supplied, it automatically defaults to 1.
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Page size (maximum number of records to return per page).
    /// Minimum value is 1. If less than 1 is supplied, it automatically defaults to 10.
    /// </summary>
    /// <example>10</example>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Comma-separated list of properties to selectively include in the shaped JSON response (Data Shaping).
    /// Leaving it empty returns all non-navigation fields of the enrollment by default.
    /// Valid shapeable fields (Case-Insensitive): enrollmentId, studentId, courseId, enrollDate, status, student (if expanded), course (if expanded).
    /// Example: "enrollmentId,status,enrollDate"
    /// </summary>
    /// <example>enrollmentId,status,enrollDate</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related navigation properties or entities to expand and include in the response (Eager Loading).
    /// Supported options (Case-Insensitive):
    /// - "student": Eagerly loads and serializes the complete personal profile details of the associated student.
    /// - "course": Eagerly loads and serializes the course details including subject and semester reference keys.
    /// Example: "student,course"
    /// </summary>
    /// <example>student,course</example>
    public string? Expand { get; set; }
}

