namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Query request parameters for searching, sorting, and paginating semesters.
/// </summary>
public class SemesterQueryRequest
{
    /// <summary>
    /// Optional search term to filter semesters by SemesterName.
    /// </summary>
    /// <example>Semester 1</example>
    public string? Search { get; set; }

    /// <summary>
    /// Sorting string format (e.g. 'semestername', '-startdate', 'enddate').
    /// </summary>
    /// <example>-startdate</example>
    public string? Sort { get; set; }

    /// <summary>
    /// The page number (1-based index) to retrieve.
    /// </summary>
    /// <example>1</example>
    public int Page { get; set; } = 1;

    /// <summary>
    /// The number of semesters per page to return.
    /// </summary>
    /// <example>10</example>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Comma-separated list of properties to include in the response (Data Shaping).
    /// </summary>
    /// <example>semesterId,semesterName</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related tables/entities to expand (e.g. 'courses').
    /// </summary>
    /// <example>courses</example>
    public string? Expand { get; set; }
}
