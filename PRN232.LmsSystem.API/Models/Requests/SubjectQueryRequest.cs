namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Container for query request parameters designed to search, filter, sort, paginate, and dynamically shape subjects list.
/// </summary>
/// <remarks>
/// This class enables powerful client-driven query operations through HTTP query string parameters. 
/// It supports full data shaping (selective fields), dynamic multi-property sorting, paginated results, and dynamic eager-loading of navigation properties.
/// </remarks>
public class SubjectQueryRequest
{
    /// <summary>
    /// Search term to filter subject records. Case-insensitive partial match against:
    /// - Subject Name (e.g. "Software Architecture", "Parallel Programming")
    /// - Subject Code (e.g. "PRN230", "PRN232")
    /// </summary>
    /// <example>PRN232</example>
    public string? Search { get; set; }

    /// <summary>
    /// Dynamic multi-property sorting expression. Comma-separated properties.
    /// Prefix a property with '-' for descending order. Default is ascending.
    /// Valid properties (Case-Insensitive): subjectId, subjectCode, subjectName, credit.
    /// Example: "subjectName,-credit"
    /// </summary>
    /// <example>subjectName,-credit</example>
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
    /// Leaving it empty returns all non-navigation fields of the subject by default.
    /// Valid shapeable fields (Case-Insensitive): subjectId, subjectCode, subjectName, credit.
    /// Example: "subjectId,subjectCode,subjectName"
    /// </summary>
    /// <example>subjectId,subjectCode,subjectName</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related navigation properties or entities to expand and include in the response (Eager Loading).
    /// Supported options (Case-Insensitive): none.
    /// </summary>
    public string? Expand { get; set; }
}

