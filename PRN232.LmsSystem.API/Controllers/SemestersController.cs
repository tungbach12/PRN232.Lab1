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
public class SemestersController : ControllerBase
{
    private readonly ISemesterService _service;

    public SemestersController(ISemesterService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of semesters with optional search, sorting, and data shaping.
    /// </summary>
    /// <param name="request">The semester query parameters, including pagination, filtering, search, and dynamic field selection.</param>
    /// <returns>A paginated list of semesters.</returns>
    /// <response code="200">Successfully retrieved the list of semesters.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResponse<object>>>> GetSemesters([FromQuery] SemesterQueryRequest request)
    {
        var result = await _service.GetSemestersAsync(SemesterMapper.ToQueryModel(request));
        var responseItems = result.Items
            .Select(model => SemesterMapper.ShapeResponse(SemesterMapper.ToResponse(model), request.Fields))
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
    /// Retrieves a specific semester by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the semester.</param>
    /// <param name="expand">Related entities to include in the response (e.g., 'courses').</param>
    /// <param name="fields">Specific fields to include in the output (data shaping).</param>
    /// <returns>The details of the requested semester.</returns>
    /// <response code="200">Successfully retrieved the semester details.</response>
    /// <response code="404">No semester was found with the specified ID.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetSemester(int id, [FromQuery] string? expand, [FromQuery] string? fields)
    {
        var semester = await _service.GetSemesterByIdAsync(id, SemesterMapper.IncludeCourses(expand));
        if (semester is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Semester not found",
                Errors = new { id }
            });
        }

        var response = SemesterMapper.ShapeResponse(SemesterMapper.ToResponse(semester), fields);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Creates a new semester in the system.
    /// </summary>
    /// <param name="request">The data required to create the new semester.</param>
    /// <returns>The details of the newly created semester.</returns>
    /// <response code="201">Successfully created the new semester.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SemesterResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SemesterResponse>>> CreateSemester([FromBody] SemesterRequest request)
    {
        var created = await _service.CreateSemesterAsync(new SemesterModel
        {
            SemesterName = request.SemesterName,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        });

        var response = SemesterMapper.ToResponse(created);
        return CreatedAtAction(nameof(GetSemester), new { id = response.SemesterId }, new ApiResponse<SemesterResponse>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Updates the details of an existing semester.
    /// </summary>
    /// <param name="id">The unique ID of the semester to update.</param>
    /// <param name="request">The updated details of the semester.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully updated the semester details.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    /// <response code="404">No semester was found with the specified ID.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateSemester(int id, [FromBody] SemesterRequest request)
    {
        var updated = await _service.UpdateSemesterAsync(id, new SemesterModel
        {
            SemesterId = id,
            SemesterName = request.SemesterName,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        });

        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Semester not found",
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
    /// Deletes an existing semester from the system.
    /// </summary>
    /// <param name="id">The unique ID of the semester to delete.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully deleted the semester.</response>
    /// <response code="404">No semester was found with the specified ID.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSemester(int id)
    {
        var deleted = await _service.DeleteSemesterAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Semester not found",
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
