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
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectsController(ISubjectService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of subjects with optional search, sorting, and data shaping.
    /// </summary>
    /// <param name="request">The subject query parameters, including pagination, filtering, search, and dynamic field selection.</param>
    /// <returns>A paginated list of subjects.</returns>
    /// <response code="200">Successfully retrieved the list of subjects.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResponse<object>>>> GetSubjects([FromQuery] SubjectQueryRequest request)
    {
        var result = await _service.GetSubjectsAsync(SubjectMapper.ToQueryModel(request));
        var responseItems = result.Items
            .Select(model => SubjectMapper.ShapeResponse(SubjectMapper.ToResponse(model), request.Fields))
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
    /// Retrieves a specific subject by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the subject.</param>
    /// <param name="expand">Related entities to include in the response (e.g., 'courses').</param>
    /// <param name="fields">Specific fields to include in the output (data shaping).</param>
    /// <returns>The details of the requested subject.</returns>
    /// <response code="200">Successfully retrieved the subject details.</response>
    /// <response code="404">No subject was found with the specified ID.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetSubject(int id, [FromQuery] string? expand, [FromQuery] string? fields)
    {
        var subject = await _service.GetSubjectByIdAsync(id);
        if (subject is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Subject not found",
                Errors = new { id }
            });
        }

        var response = SubjectMapper.ShapeResponse(SubjectMapper.ToResponse(subject), fields);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Creates a new subject in the system.
    /// </summary>
    /// <param name="request">The data required to create the new subject.</param>
    /// <returns>The details of the newly created subject.</returns>
    /// <response code="201">Successfully created the new subject.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<SubjectResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<SubjectResponse>>> CreateSubject([FromBody] SubjectRequest request)
    {
        var created = await _service.CreateSubjectAsync(new SubjectModel
        {
            SubjectCode = request.SubjectCode,
            SubjectName = request.SubjectName,
            Credit = request.Credit
        });

        var response = SubjectMapper.ToResponse(created);
        return CreatedAtAction(nameof(GetSubject), new { id = response.SubjectId }, new ApiResponse<SubjectResponse>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Updates the details of an existing subject.
    /// </summary>
    /// <param name="id">The unique ID of the subject to update.</param>
    /// <param name="request">The updated details of the subject.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully updated the subject details.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    /// <response code="404">No subject was found with the specified ID.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateSubject(int id, [FromBody] SubjectRequest request)
    {
        var updated = await _service.UpdateSubjectAsync(id, new SubjectModel
        {
            SubjectId = id,
            SubjectCode = request.SubjectCode,
            SubjectName = request.SubjectName,
            Credit = request.Credit
        });

        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Subject not found",
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
    /// Deletes an existing subject from the system.
    /// </summary>
    /// <param name="id">The unique ID of the subject to delete.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully deleted the subject.</response>
    /// <response code="404">No subject was found with the specified ID.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSubject(int id)
    {
        var deleted = await _service.DeleteSubjectAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Subject not found",
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
