namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// A standardized paginated response wrapper containing list items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the list.</typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// The collection of data items for the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

    /// <summary>
    /// The metadata containing pagination statistics.
    /// </summary>
    public PaginationMetadata Pagination { get; set; } = new();
}

/// <summary>
/// Metadata describing pagination parameters and totals.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// The current page number (1-based).
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The size of the retrieved page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The total number of items matching the query.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// The total number of pages calculated from the page size.
    /// </summary>
    public int TotalPages { get; set; }
}
