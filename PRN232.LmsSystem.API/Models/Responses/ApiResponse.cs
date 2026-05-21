namespace PRN232.Lab1.Models.Responses;

/// <summary>
/// A standardized wrapper for all API responses.
/// </summary>
/// <typeparam name="T">The type of the data returned by the API.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates whether the API request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// A descriptive message about the response or error details.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The response payload.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Structured error details if the request failed.
    /// </summary>
    public object? Errors { get; set; }
}
