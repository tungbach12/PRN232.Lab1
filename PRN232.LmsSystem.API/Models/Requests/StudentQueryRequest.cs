namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Query request parameters for searching, sorting, and paginating students.
/// </summary>
public class StudentQueryRequest
{
    /// <summary>
    /// Optional search term to filter students by FullName or Email.
    /// </summary>
    /// <example>John</example>
    public string? Search { get; set; }

    /// <summary>
    /// Sorting string format (e.g. 'fullname', '-fullname', 'email').
    /// </summary>
    /// <example>-fullname</example>
    public string? Sort { get; set; }

    /// <summary>
    /// The page number (1-based index) to retrieve.
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of students per page to return.
    /// </summary>
    /// <example>10</example>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Comma-separated list of properties to include in the response (Data Shaping).
    /// </summary>
    /// <example>studentId,fullName</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related tables/entities to expand (e.g. 'enrollments').
    /// </summary>
    /// <example>enrollments</example>
    public string? Expand { get; set; }
}
