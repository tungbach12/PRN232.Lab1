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
            Size = request.Size
        };
    }


    public static SubjectResponse ToResponse(SubjectModel model)
    {
        return new SubjectResponse
        {
            SubjectId = model.SubjectId,
            SubjectCode = model.SubjectCode,
            SubjectName = model.SubjectName,
            Credit = model.Credit
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
