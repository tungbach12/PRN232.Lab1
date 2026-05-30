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
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;

    public CoursesController(ICourseService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retrieves a paginated list of courses with optional search, sorting, and data shaping.
    /// </summary>
    /// <param name="request">The course query parameters, including pagination, filtering, search, and dynamic field selection.</param>
    /// <returns>A paginated list of courses.</returns>
    /// <response code="200">Successfully retrieved the list of courses.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<object>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResponse<object>>>> GetCourses([FromQuery] CourseQueryRequest request)
    {
        var result = await _service.GetCoursesAsync(CourseMapper.ToQueryModel(request));
        var responseItems = result.Items
            .Select(model => CourseMapper.ShapeResponse(CourseMapper.ToResponse(model), request.Fields))
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
    /// Retrieves a specific course by its unique identifier.
    /// </summary>
    /// <param name="id">The unique ID of the course.</param>
    /// <param name="expand">Related entities to include in the response (e.g., 'semester', 'subject', 'enrollments').</param>
    /// <param name="fields">Specific fields to include in the output (data shaping).</param>
    /// <returns>The details of the requested course.</returns>
    /// <response code="200">Successfully retrieved the course details.</response>
    /// <response code="404">No course was found with the specified ID.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> GetCourse(int id, [FromQuery] string? expand, [FromQuery] string? fields)
    {
        var course = await _service.GetCourseByIdAsync(
            id,
            CourseMapper.IncludeSemester(expand),
            CourseMapper.IncludeEnrollments(expand));

        if (course is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Course not found",
                Errors = new { id }
            });
        }

        var response = CourseMapper.ShapeResponse(CourseMapper.ToResponse(course), fields);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Creates a new course in the system.
    /// </summary>
    /// <param name="request">The data required to create the new course.</param>
    /// <returns>The details of the newly created course.</returns>
    /// <response code="201">Successfully created the new course.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CourseResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> CreateCourse([FromBody] CourseRequest request)
    {
        var created = await _service.CreateCourseAsync(new CourseModel
        {
            CourseName = request.CourseName,
            SemesterId = request.SemesterId
        });

        var response = CourseMapper.ToResponse(created);
        return CreatedAtAction(nameof(GetCourse), new { id = response.CourseId }, new ApiResponse<CourseResponse>
        {
            Success = true,
            Message = "Request processed successfully",
            Data = response
        });
    }

    /// <summary>
    /// Updates the details of an existing course.
    /// </summary>
    /// <param name="id">The unique ID of the course to update.</param>
    /// <param name="request">The updated details of the course.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully updated the course details.</response>
    /// <response code="400">The request body is malformed or validation failed.</response>
    /// <response code="404">No course was found with the specified ID.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> UpdateCourse(int id, [FromBody] CourseRequest request)
    {
        var updated = await _service.UpdateCourseAsync(id, new CourseModel
        {
            CourseId = id,
            CourseName = request.CourseName,
            SemesterId = request.SemesterId
        });

        if (!updated)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Course not found",
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
    /// Deletes an existing course from the system.
    /// </summary>
    /// <param name="id">The unique ID of the course to delete.</param>
    /// <returns>An API response indicating success or failure.</returns>
    /// <response code="200">Successfully deleted the course.</response>
    /// <response code="404">No course was found with the specified ID.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCourse(int id)
    {
        var deleted = await _service.DeleteCourseAsync(id);
        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Course not found",
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
    /// Retrieves a paginated list of enrollments for a specific course.
    /// </summary>
    /// <param name="courseId">The unique ID of the course.</param>
    /// <param name="request">The enrollment query parameters, including pagination, filtering, search, and data shaping.</param>
    /// <returns>A paginated list of enrollments.</returns>
    /// <response code="200">Successfully retrieved the list of enrollments.</response>
    /// <response code="404">No course was found with the specified ID.</response>
    [HttpGet("{courseId:int}/enrollments")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<object>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PagedResponse<object>>>> GetCourseEnrollments(int courseId, [FromQuery] EnrollmentQueryRequest request)
    {
        var course = await _service.GetCourseByIdAsync(courseId, includeSemester: false, includeEnrollments: false);
        if (course is null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Course not found",
                Errors = new { courseId }
            });
        }

        var result = await _service.GetEnrollmentByCourseId(courseId, EnrollmentMapper.ToQueryModel(request));
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
}
