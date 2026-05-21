using PRN232.Lab1.Models.Requests;
using PRN232.Lab1.Models.Responses;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.Lab1.Mappings;

public static class EnrollmentMapper
{
    public static EnrollmentQueryModel ToQueryModel(EnrollmentQueryRequest request)
    {
        return new EnrollmentQueryModel
        {
            Search = request.Search,
            Sort = request.Sort,
            Page = request.Page,
            Size = request.Size,
            IncludeStudent = HasExpand(request.Expand, "student"),
            IncludeCourse = HasExpand(request.Expand, "course")
        };
    }

    public static bool IncludeStudent(string? expand)
    {
        return HasExpand(expand, "student");
    }

    public static bool IncludeCourse(string? expand)
    {
        return HasExpand(expand, "course");
    }

    public static EnrollmentResponse ToResponse(EnrollmentModel model)
    {
        return new EnrollmentResponse
        {
            EnrollmentId = model.EnrollmentId,
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            EnrollDate = model.EnrollDate,
            Status = model.Status,
            Student = model.Student is null ? null : new StudentResponse
            {
                StudentId = model.Student.StudentId,
                FullName = model.Student.FullName,
                Email = model.Student.Email,
                DateOfBirth = model.Student.DateOfBirth
            },
            Course = model.Course is null ? null : new CourseResponse
            {
                CourseId = model.Course.CourseId,
                CourseName = model.Course.CourseName,
                SemesterId = model.Course.SemesterId,
                SubjectId = model.Course.SubjectId
            }
        };
    }

    public static object ShapeResponse(EnrollmentResponse response, string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
        {
            return response;
        }

        var selected = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(f => f.ToLowerInvariant())
            .ToHashSet();

        var shaped = new Dictionary<string, object?>();

        if (selected.Contains("enrollmentid"))
        {
            shaped["enrollmentId"] = response.EnrollmentId;
        }

        if (selected.Contains("studentid"))
        {
            shaped["studentId"] = response.StudentId;
        }

        if (selected.Contains("courseid"))
        {
            shaped["courseId"] = response.CourseId;
        }

        if (selected.Contains("enrolldate"))
        {
            shaped["enrollDate"] = response.EnrollDate;
        }

        if (selected.Contains("status"))
        {
            shaped["status"] = response.Status;
        }

        if (selected.Contains("student"))
        {
            shaped["student"] = response.Student;
        }

        if (selected.Contains("course"))
        {
            shaped["course"] = response.Course;
        }

        return shaped;
    }

    private static bool HasExpand(string? expand, string name)
    {
        if (string.IsNullOrWhiteSpace(expand))
        {
            return false;
        }

        return expand.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Any(item => string.Equals(item, name, StringComparison.OrdinalIgnoreCase));
    }
}
