using Microsoft.AspNetCore.Mvc;
using PRN232.Lab1.Mappings;
using PRN232.Lab1.Models.Requests;
using PRN232.Lab1.Models.Responses;
using PRN232.LmsSystem.Services.Models;
using PRN232.LmsSystem.Services.Services;

namespace PRN232.Lab1.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _service;

    public EnrollmentsController(IEnrollmentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of enrollments with optional search, sorting, and data shaping.
    /// </summary>
    /// <param name="request">The enrollment query parameters, including pagination, filtering, search, and dynamic field selection.</param>
    /// <returns>A paginated list of enrollments.</returns>
    /// <response code="200">Successfully retrieved the list of enrollments.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResponse<object>>>> GetEnrollments([FromQuery] EnrollmentQueryRequest request)
    {
        var result = await _service.GetEnrollmentsAsync(EnrollmentMapper.ToQueryModel(request));
        var responseItems = result.Items
            .Select(model => EnrollmentMapper.ShapeResponse(EnrollmentMapper.ToResponse(model), request.Fields))
            .ToList();

        var pagedResponse = new PagedResponse<object>
        {
            Items = responseItems,
            Pagination = new PaginationMetadata
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages
            }
        };

        return Ok(new ApiResponse<PagedResponse<object>>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = pagedResponse
        });
    }

    /// <summary>
    /// Retrieves a specific enrollment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the enrollment.</param>
    /// <param name="expand">Related entities to include in the response (e.g., 'student', 'course').</param>
    /// <param name="fields">Specific fields to include in the output (data shaping).</param>
    /// <returns>The details of the requested enrollment.</returns>
    /// <response code="200">Successfully retrieved the enrollment details.</response>
    /// <response code="404">No enrollment was found with the specified ID.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetEnrollment(int id, [FromQuery] string? expand, [FromQuery] string? fields)
    {
        var enrollment = await _service.GetEnrollmentByIdAsync(id, EnrollmentMapper.IncludeStudent(expand), EnrollmentMapper.IncludeCourse(expand));
        if (enrollment is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found",
                Errors = new { id }
            });
        }

        var response = EnrollmentMapper.ShapeResponse(EnrollmentMapper.ToResponse(enrollment), fields);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Creates a new student enrollment.
    /// </summary>
    /// <param name="request">The data required to create the new enrollment.</param>
    /// <returns>The details of the newly created enrollment.</returns>
    /// <response code="201">Successfully created the new enrollment.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EnrollmentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<EnrollmentResponse>>> CreateEnrollment([FromBody] EnrollmentRequest request)
    {
        var created = await _service.CreateEnrollmentAsync(new EnrollmentModel
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            EnrollDate = request.EnrollDate,
            Status = request.Status
        });

        var response = EnrollmentMapper.ToResponse(created);
        return CreatedAtAction(nameof(GetEnrollment), new { id = response.EnrollmentId }, new ApiResponse<EnrollmentResponse>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Updates the details of an existing enrollment.
    /// </summary>
    /// <param name="id">The unique ID of the enrollment to update.</param>
    /// <param name="request">The updated details of the enrollment.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully updated the enrollment details.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    /// <response code="404">No enrollment was found with the specified ID.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateEnrollment(int id, [FromBody] EnrollmentRequest request)
    {
        var updated = await _service.UpdateEnrollmentAsync(id, new EnrollmentModel
        {
            EnrollmentId = id,
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            EnrollDate = request.EnrollDate,
            Status = request.Status
        });

        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found",
                Errors = new { id }
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully"
        });
    }

    /// <summary>
    /// Deletes an existing enrollment from the system.
    /// </summary>
    /// <param name="id">The unique ID of the enrollment to delete.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully deleted the enrollment.</response>
    /// <response code="404">No enrollment was found with the specified ID.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteEnrollment(int id)
    {
        var deleted = await _service.DeleteEnrollmentAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found",
                Errors = new { id }
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully"
        });
    }
}
