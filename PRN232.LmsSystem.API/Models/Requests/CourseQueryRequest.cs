namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Container for query request parameters designed to search, filter, sort, paginate, and dynamically shape courses list.
/// </summary>
public class CourseQueryRequest
{
    /// <summary>
    /// Search term to filter course records. Case-insensitive partial match against:
    /// - Course Name (e.g. "Introduction to C#", "Software Architecture")
    /// </summary>
    /// <example>C#</example>
    public string? Search { get; set; }

    /// <summary>
    /// Dynamic multi-property sorting expression. Comma-separated properties.
    /// Prefix a property with '-' for descending order. Default is ascending.
    /// Valid properties (Case-Insensitive): courseId, courseName, semesterId, subjectId.
    /// Example: "courseName,-semesterId"
    /// </summary>
    /// <example>courseName</example>
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
    /// Comma-separated list of properties to include in the shaped JSON response (Data Shaping).
    /// Leaving it empty returns all non-navigation properties by default.
    /// Valid shapeable fields (Case-Insensitive): courseId, courseName, semesterId, subjectId, semester (if expanded), subject (if expanded), enrollments (if expanded).
    /// Example: "courseId,courseName"
    /// </summary>
    /// <example>courseId,courseName</example>
    public string? Fields { get; set; }

    /// <summary>
    /// Comma-separated list of related navigation properties or entities to eager-load and include in the payload.
    /// Supported options (Case-Insensitive):
    /// - "semester": Associated semester details (semesterName, startDate, endDate).
    /// - "subject": Associated subject details (subjectCode, subjectName, credit).
    /// - "enrollments": Enrollment records for students in this course.
    /// Example: "semester,subject"
    /// </summary>
    /// <example>semester,subject</example>
    public string? Expand { get; set; }
}


