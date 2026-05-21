using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Request model for creating or updating a semester.
/// </summary>
public class SemesterRequest
{
    /// <summary>
    /// Name of the semester (e.g., "Semester 1").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SemesterName { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the semester.
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the semester.
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }
}
