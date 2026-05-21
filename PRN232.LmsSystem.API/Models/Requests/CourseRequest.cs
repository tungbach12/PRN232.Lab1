using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Request model for creating or updating a course.
/// </summary>
public class CourseRequest
{
    /// <summary>
    /// Name of the course (e.g., "Course 1").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the semester this course belongs to.
    /// </summary>
    [Required]
    public int SemesterId { get; set; }

    /// <summary>
    /// The ID of the subject this course belongs to.
    /// </summary>
    [Required]
    public int SubjectId { get; set; }
}
