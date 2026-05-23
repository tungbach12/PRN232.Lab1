using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace PRN232.LmsSystem.Services.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Dynamically applies sorting to any IQueryable query using System.Linq.Dynamic.Core.
    /// Handles multiple sorting fields (e.g., "fullname, -email") where '-' denotes descending order.
    /// </summary>
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return source;
        }

        var sortFields = sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var orderExpressions = sortFields.Select(field =>
        {
            var descending = field.StartsWith('-');
            var propertyName = descending ? field[1..] : field;
            return $"{propertyName} {(descending ? "desc" : "asc")}";
        });

        var sortExpression = string.Join(", ", orderExpressions);
        return source.OrderBy(sortExpression);
    }
}
