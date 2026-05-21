using PRN232.Lab1.Models.Requests;
using PRN232.Lab1.Models.Responses;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.Lab1.Mappings;

public static class SemesterMapper
{
    public static SemesterQueryModel ToQueryModel(SemesterQueryRequest request)
    {
        return new SemesterQueryModel
        {
            Search = request.Search,
            Sort = request.Sort,
            Page = request.Page,
            Size = request.Size,
            IncludeCourses = HasExpand(request.Expand, "courses")
        };
    }

    public static bool IncludeCourses(string? expand)
    {
        return HasExpand(expand, "courses");
    }

    public static SemesterResponse ToResponse(SemesterModel model)
    {
        return new SemesterResponse
        {
            SemesterId = model.SemesterId,
            SemesterName = model.SemesterName,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Courses = model.Courses?.Select(c => new CourseResponse
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                SemesterId = c.SemesterId,
                SubjectId = c.SubjectId
            }).ToList()
        };
    }

    public static object ShapeResponse(SemesterResponse response, string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
        {
            return response;
        }

        var selected = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(f => f.ToLowerInvariant())
            .ToHashSet();

        var shaped = new Dictionary<string, object?>();

        if (selected.Contains("semesterid"))
        {
            shaped["semesterId"] = response.SemesterId;
        }

        if (selected.Contains("semestername"))
        {
            shaped["semesterName"] = response.SemesterName;
        }

        if (selected.Contains("startdate"))
        {
            shaped["startDate"] = response.StartDate;
        }

        if (selected.Contains("enddate"))
        {
            shaped["endDate"] = response.EndDate;
        }

        if (selected.Contains("courses"))
        {
            shaped["courses"] = response.Courses;
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
