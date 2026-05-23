namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Container for query request parameters designed to search, filter, sort, paginate, and dynamically shape semesters list.
/// </summary>
/// <remarks>
/// This class enables powerful client-driven query operations through HTTP query string parameters. 
/// It supports full data shaping (selective fields), dynamic multi-property sorting, paginated results, and dynamic eager-loading of navigation properties.
/// </remarks>
public class SemesterQueryRequest
{
    /// <summary>
    /// Search term to filter semesters. Case-insensitive partial match against:
    /// - Semester Name (e.g. "Summer 2026", "Fall 2026")
    /// </summary>
    /// <example>Summer</example>
    public string? Search { get; set; }

    /// <summary>
    /// Dynamic multi-property sorting expression. Comma-separated properties.
    /// Prefix a property with '-' for descending order. Default is ascending.
    /// Valid properties (Case-Insensitive): semesterId, semesterName, startDate, endDate.
    /// Example: "semesterName,-startDate"
    /// </summary>
    /// <example>semesterName,-startDate</example>
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
    /// Leaving it empty returns all non-navigation fields of the semester by default.
    /// Valid shapeable fields (Case-Insensitive): semesterId, semesterName, startDate, endDate, courses (if expanded).
    /// Example: "semesterId,semesterName,startDate"
    /// </summary>
    /// <example>semesterId,semesterName,startDate</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related navigation properties or entities to expand and include in the response (Eager Loading).
    /// Supported options (Case-Insensitive):
    /// - "courses": Eagerly loads the list of courses associated with this semester.
    /// Example: "courses"
    /// </summary>
    /// <example>courses</example>
    public string? Expand { get; set; }
}
