namespace PRN232.LmsSystem.Services.Models;

public class EnrollmentQueryModel
{
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public bool IncludeStudent { get; set; }
    public bool IncludeCourse { get; set; }
}
