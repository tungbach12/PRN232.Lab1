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
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of students with optional search, sorting, and data shaping.
    /// </summary>
    /// <param name="request">The student query parameters, including pagination, filtering, search, and dynamic field selection.</param>
    /// <returns>A paginated list of students.</returns>
    /// <response code="200">Successfully retrieved the list of students.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResponse<object>>>> GetStudents([FromQuery] StudentQueryRequest request)
    {
        var result = await _service.GetStudentsAsync(StudentMapper.ToQueryModel(request));
        var responseItems = result.Items
            .Select(model => StudentMapper.ShapeResponse(StudentMapper.ToResponse(model), request.Fields))
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
    /// Retrieves a specific student by their unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the student.</param>
    /// <param name="expand">Related entities to include in the response (e.g., 'enrollments').</param>
    /// <param name="fields">Specific fields to include in the output (data shaping).</param>
    /// <returns>The details of the requested student.</returns>
    /// <response code="200">Successfully retrieved the student details.</response>
    /// <response code="404">No student was found with the specified ID.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetStudent(int id, [FromQuery] string? expand, [FromQuery] string? fields)
    {
        var student = await _service.GetStudentByIdAsync(id, StudentMapper.IncludeEnrollments(expand));
        if (student is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found",
                Errors = new { id }
            });
        }

        var response = StudentMapper.ShapeResponse(StudentMapper.ToResponse(student), fields);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Creates a new student in the system.
    /// </summary>
    /// <param name="request">The data required to create the new student.</param>
    /// <returns>The details of the newly created student.</returns>
    /// <response code="201">Successfully created the new student.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StudentResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<StudentResponse>>> CreateStudent([FromBody] StudentRequest request)
    {
        var created = await _service.CreateStudentAsync(new StudentModel
        {
            FullName = request.FullName,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth
        });

        var response = StudentMapper.ToResponse(created);
        return CreatedAtAction(nameof(GetStudent), new { id = response.StudentId }, new ApiResponse<StudentResponse>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Updates the details of an existing student.
    /// </summary>
    /// <param name="id">The unique ID of the student to update.</param>
    /// <param name="request">The updated details of the student.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully updated the student details.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    /// <response code="404">No student was found with the specified ID.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateStudent(int id, [FromBody] StudentRequest request)
    {
        var updated = await _service.UpdateStudentAsync(id, new StudentModel
        {
            StudentId = id,
            FullName = request.FullName,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth
        });

        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found",
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
    /// Deletes an existing student from the system.
    /// </summary>
    /// <param name="id">The unique ID of the student to delete.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully deleted the student.</response>
    /// <response code="404">No student was found with the specified ID.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteStudent(int id)
    {
        var deleted = await _service.DeleteStudentAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found",
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
