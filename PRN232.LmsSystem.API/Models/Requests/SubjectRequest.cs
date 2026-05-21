using System.ComponentModel.DataAnnotations;

namespace PRN232.Lab1.Models.Requests;

/// <summary>
/// Request model for creating or updating a subject.
/// </summary>
public class SubjectRequest
{
    /// <summary>
    /// Unique code for the subject (e.g., "SUBJ001").
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string SubjectCode { get; set; } = string.Empty;

    /// <summary>
    /// Name of the subject (e.g., "Mathematics").
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SubjectName { get; set; } = string.Empty;

    /// <summary>
    /// Number of credits for the subject.
    /// </summary>
    [Required]
    [Range(1, 10)]
    public int Credit { get; set; }
}
