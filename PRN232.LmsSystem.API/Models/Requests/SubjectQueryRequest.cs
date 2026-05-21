namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Query request parameters for searching, sorting, and paginating subjects.
/// </summary>
public class SubjectQueryRequest
{
    /// <summary>
    /// Optional search term to filter subjects by SubjectName or SubjectCode.
    /// </summary>
    /// <example>Math</example>
    public string? Search { get; set; }

    /// <summary>
    /// Sorting string format (e.g. 'subjectname', '-subjectcode', 'credit').
    /// </summary>
    /// <example>subjectname</example>
    public string? Sort { get; set; }

    /// <summary>
    /// The page number (1-based index) to retrieve.
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of subjects per page to return.
    /// </summary>
    /// <example>10</example>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Comma-separated list of properties to include in the response (Data Shaping).
    /// </summary>
    /// <example>subjectId,subjectName</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related tables/entities to expand (e.g. 'courses').
    /// </summary>
    /// <example>courses</example>
    public string? Expand { get; set; }
}
