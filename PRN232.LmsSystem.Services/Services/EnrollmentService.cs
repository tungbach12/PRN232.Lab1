using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Extensions;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repository;

    public EnrollmentService(IEnrollmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<EnrollmentModel>> GetEnrollmentsAsync(EnrollmentQueryModel query)
    {
        var includeStudent = query.IncludeStudent;
        var includeCourse = query.IncludeCourse;
        var enrollmentsQuery = _repository.Query(includeStudent, includeCourse);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            enrollmentsQuery = enrollmentsQuery.Where(e => 
                e.Status.Contains(keyword) || 
                (e.Student != null && e.Student.FullName.Contains(keyword)) ||
                (e.Course != null && e.Course.CourseName.Contains(keyword)));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            enrollmentsQuery = enrollmentsQuery.ApplySort(query.Sort);
        }

        var totalItems = await enrollmentsQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var enrollments = await enrollmentsQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<EnrollmentModel>
        {
            Items = enrollments.Select(MapToModel).ToList(),
            TotalItems = totalItems,
            Page = page,
            PageSize = size,
            TotalPages = totalPages
        };
    }

    public async Task<EnrollmentModel?> GetEnrollmentByIdAsync(int id, bool includeStudent, bool includeCourse)
    {
        var enrollment = await _repository.GetByIdAsync(id, includeStudent, includeCourse);
        return enrollment is null ? null : MapToModel(enrollment);
    }

    public async Task<EnrollmentModel> CreateEnrollmentAsync(EnrollmentModel model)
    {
        var entity = new Enrollment
        {
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            EnrollDate = model.EnrollDate == default ? DateTime.UtcNow : model.EnrollDate,
            Status = string.IsNullOrWhiteSpace(model.Status) ? "Active" : model.Status
        };

        var created = await _repository.AddAsync(entity);
        return MapToModel(created);
    }

    public async Task<bool> UpdateEnrollmentAsync(int id, EnrollmentModel model)
    {
        var entity = new Enrollment
        {
            EnrollmentId = id,
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            EnrollDate = model.EnrollDate,
            Status = model.Status
        };

        return await _repository.UpdateAsync(id, entity);
    }

    public Task<bool> DeleteEnrollmentAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }



    private static EnrollmentModel MapToModel(Enrollment enrollment)
    {
        return new EnrollmentModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status,
            Student = enrollment.Student is null ? null : new StudentModel
            {
                StudentId = enrollment.Student.StudentId,
                FullName = enrollment.Student.FullName,
                Email = enrollment.Student.Email,
                DateOfBirth = enrollment.Student.DateOfBirth
            },
            Course = enrollment.Course is null ? null : new CourseModel
            {
                CourseId = enrollment.Course.CourseId,
                CourseName = enrollment.Course.CourseName,
                SemesterId = enrollment.Course.SemesterId
            }
        };
    }
}
