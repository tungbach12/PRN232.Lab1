namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Container for query request parameters designed to search, filter, sort, paginate, and dynamically shape students list.
/// </summary>
/// <remarks>
/// This class enables powerful client-driven query operations through HTTP query string parameters. 
/// It supports full data shaping (selective fields), dynamic multi-property sorting, paginated results, and dynamic eager-loading of navigation properties.
/// </remarks>
public class StudentQueryRequest
{
    /// <summary>
    /// Search term to filter student profiles. Case-insensitive partial match against:
    /// - Student's Full Name (e.g. "John Doe")
    /// - Student's Email address (e.g. "john.doe@example.com")
    /// </summary>
    /// <example>John</example>
    public string? Search { get; set; }

    /// <summary>
    /// Dynamic multi-property sorting expression. Comma-separated properties.
    /// Prefix a property with '-' for descending order. Default is ascending.
    /// Valid properties (Case-Insensitive): studentId, fullName, email, dateOfBirth.
    /// Example: "fullName,-dateOfBirth"
    /// </summary>
    /// <example>fullName,-dateOfBirth</example>
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
    /// Leaving it empty returns all non-navigation fields of the student by default.
    /// Valid shapeable fields (Case-Insensitive): studentId, fullName, email, dateOfBirth, enrollments (if expanded).
    /// Example: "studentId,fullName,email"
    /// </summary>
    /// <example>studentId,fullName,email</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related navigation properties or entities to expand and include in the response (Eager Loading).
    /// Supported options (Case-Insensitive):
    /// - "enrollments": Eagerly loads the complete list of enrollment records (along with associated courses) belonging to the student.
    /// Example: "enrollments"
    /// </summary>
    /// <example>enrollments</example>
    public string? Expand { get; set; }
}

