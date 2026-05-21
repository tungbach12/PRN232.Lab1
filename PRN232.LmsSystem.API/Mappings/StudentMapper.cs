using PRN232.Lab1.Models.Requests;
using PRN232.Lab1.Models.Responses;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.Lab1.Mappings;

public static class StudentMapper
{
    public static StudentQueryModel ToQueryModel(StudentQueryRequest request)
    {
        return new StudentQueryModel
        {
            Search = request.Search,
            Sort = request.Sort,
            Page = request.Page,
            Size = request.Size,
            IncludeEnrollments = HasExpand(request.Expand, "enrollments")
        };
    }

    public static bool IncludeEnrollments(string? expand)
    {
        return HasExpand(expand, "enrollments");
    }

    public static StudentResponse ToResponse(StudentModel model)
    {
        return new StudentResponse
        {
            StudentId = model.StudentId,
            FullName = model.FullName,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth,
            Enrollments = model.Enrollments?.Select(e => new EnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                CourseId = e.CourseId,
                Course = e.Course is null ? null : new CourseResponse
                {
                    CourseId = e.Course.CourseId,
                    CourseName = e.Course.CourseName,
                    SemesterId = e.Course.SemesterId,
                    SubjectId = e.Course.SubjectId
                },
                EnrollDate = e.EnrollDate,
                Status = e.Status
            }).ToList()
        };
    }

    public static object ShapeResponse(StudentResponse response, string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
        {
            return response;
        }

        var selected = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(f => f.ToLowerInvariant())
            .ToHashSet();

        var shaped = new Dictionary<string, object?>();

        if (selected.Contains("studentid"))
        {
            shaped["studentId"] = response.StudentId;
        }

        if (selected.Contains("fullname"))
        {
            shaped["fullName"] = response.FullName;
        }

        if (selected.Contains("email"))
        {
            shaped["email"] = response.Email;
        }

        if (selected.Contains("dateofbirth"))
        {
            shaped["dateOfBirth"] = response.DateOfBirth;
        }

        if (selected.Contains("enrollments"))
        {
            shaped["enrollments"] = response.Enrollments;
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
