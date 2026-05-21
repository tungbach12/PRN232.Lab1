using PRN232.Lab1.Models.Requests;
using PRN232.Lab1.Models.Responses;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.Lab1.Mappings;

public static class SubjectMapper
{
    public static SubjectQueryModel ToQueryModel(SubjectQueryRequest request)
    {
        return new SubjectQueryModel
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

    public static SubjectResponse ToResponse(SubjectModel model)
    {
        return new SubjectResponse
        {
            SubjectId = model.SubjectId,
            SubjectCode = model.SubjectCode,
            SubjectName = model.SubjectName,
            Credit = model.Credit,
            Courses = model.Courses?.Select(c => new CourseResponse
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                SemesterId = c.SemesterId,
                SubjectId = c.SubjectId
            }).ToList()
        };
    }

    public static object ShapeResponse(SubjectResponse response, string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
        {
            return response;
        }

        var selected = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(f => f.ToLowerInvariant())
            .ToHashSet();

        var shaped = new Dictionary<string, object?>();

        if (selected.Contains("subjectid"))
        {
            shaped["subjectId"] = response.SubjectId;
        }

        if (selected.Contains("subjectcode"))
        {
            shaped["subjectCode"] = response.SubjectCode;
        }

        if (selected.Contains("subjectname"))
        {
            shaped["subjectName"] = response.SubjectName;
        }

        if (selected.Contains("credit"))
        {
            shaped["credit"] = response.Credit;
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
