using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Request model for creating or updating a student enrollment.
/// </summary>
public class EnrollmentRequest
{
    /// <summary>
    /// The unique identifier of the enrolled student.
    /// </summary>
    /// <example>1</example>
    [Required]
    public int StudentId { get; set; }

    /// <summary>
    /// The unique identifier of the course.
    /// </summary>
    /// <example>5</example>
    [Required]
    public int CourseId { get; set; }

    /// <summary>
    /// The date the enrollment was created.
    /// </summary>
    /// <example>2026-05-20</example>
    public DateTime EnrollDate { get; set; }

    /// <summary>
    /// The status of the enrollment (e.g. 'Active', 'Completed', 'Dropped').
    /// </summary>
    /// <example>Active</example>
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}
