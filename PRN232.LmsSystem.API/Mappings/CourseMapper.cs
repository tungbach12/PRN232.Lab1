using PRN232.Lab1.Models.Requests;
using PRN232.Lab1.Models.Responses;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.Lab1.Mappings;

public static class CourseMapper
{
    public static CourseQueryModel ToQueryModel(CourseQueryRequest request)
    {
        return new CourseQueryModel
        {
            Search = request.Search,
            Sort = request.Sort,
            Page = request.Page,
            Size = request.Size,
            IncludeSemester = HasExpand(request.Expand, "semester"),
            IncludeSubject = HasExpand(request.Expand, "subject"),
            IncludeEnrollments = HasExpand(request.Expand, "enrollments")
        };
    }

    public static bool IncludeSemester(string? expand)
    {
        return HasExpand(expand, "semester");
    }

    public static bool IncludeSubject(string? expand)
    {
        return HasExpand(expand, "subject");
    }

    public static bool IncludeEnrollments(string? expand)
    {
        return HasExpand(expand, "enrollments");
    }

    public static CourseResponse ToResponse(CourseModel model)
    {
        return new CourseResponse
        {
            CourseId = model.CourseId,
            CourseName = model.CourseName,
            SemesterId = model.SemesterId,
            SubjectId = model.SubjectId,
            Semester = model.Semester is null ? null : new SemesterResponse
            {
                SemesterId = model.Semester.SemesterId,
                SemesterName = model.Semester.SemesterName,
                StartDate = model.Semester.StartDate,
                EndDate = model.Semester.EndDate
            },
            Subject = model.Subject is null ? null : new SubjectResponse
            {
                SubjectId = model.Subject.SubjectId,
                SubjectCode = model.Subject.SubjectCode,
                SubjectName = model.Subject.SubjectName,
                Credit = model.Subject.Credit
            },
            Enrollments = model.Enrollments?.Select(e => new EnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentId = e.StudentId,
                CourseId = e.CourseId,
                EnrollDate = e.EnrollDate,
                Status = e.Status,
                Student = e.Student is null ? null : new StudentResponse
                {
                    StudentId = e.Student.StudentId,
                    FullName = e.Student.FullName,
                    Email = e.Student.Email,
                    DateOfBirth = e.Student.DateOfBirth
                }
            }).ToList()
        };
    }

    public static object ShapeResponse(CourseResponse response, string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
        {
            return response;
        }

        var selected = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(f => f.ToLowerInvariant())
            .ToHashSet();

        var shaped = new Dictionary<string, object?>();

        if (selected.Contains("courseid"))
        {
            shaped["courseId"] = response.CourseId;
        }

        if (selected.Contains("coursename"))
        {
            shaped["courseName"] = response.CourseName;
        }

        if (selected.Contains("semesterid"))
        {
            shaped["semesterId"] = response.SemesterId;
        }

        if (selected.Contains("subjectid"))
        {
            shaped["subjectId"] = response.SubjectId;
        }

        if (selected.Contains("semester"))
        {
            shaped["semester"] = response.Semester;
        }

        if (selected.Contains("subject"))
        {
            shaped["subject"] = response.Subject;
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
