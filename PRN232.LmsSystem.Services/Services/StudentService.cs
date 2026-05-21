using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<StudentModel>> GetStudentsAsync(StudentQueryModel query)
    {
        var includeEnrollments = query.IncludeEnrollments;
        var studentsQuery = _repository.Query(includeEnrollments);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            studentsQuery = studentsQuery.Where(s => s.FullName.Contains(keyword) || s.Email.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            studentsQuery = ApplySorting(studentsQuery, query.Sort);
        }

        var totalItems = await studentsQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var students = await studentsQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<StudentModel>
        {
            Items = students.Select(MapToModel).ToList(),
            TotalItems = totalItems,
            Page = page,
            PageSize = size,
            TotalPages = totalPages
        };
    }

    public async Task<StudentModel?> GetStudentByIdAsync(int id, bool includeEnrollments)
    {
        var student = await _repository.GetByIdAsync(id, includeEnrollments);
        return student is null ? null : MapToModel(student);
    }

    public async Task<StudentModel> CreateStudentAsync(StudentModel model)
    {
        var entity = new Student
        {
            FullName = model.FullName,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth
        };

        var created = await _repository.AddAsync(entity);
        return MapToModel(created);
    }

    public async Task<bool> UpdateStudentAsync(int id, StudentModel model)
    {
        var entity = new Student
        {
            StudentId = id,
            FullName = model.FullName,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth
        };

        return await _repository.UpdateAsync(id, entity);
    }

    public Task<bool> DeleteStudentAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static IQueryable<Student> ApplySorting(IQueryable<Student> query, string sort)
    {
        var sortFields = sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IOrderedQueryable<Student>? ordered = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith('-');
            var name = descending ? field[1..] : field;

            ordered = name.ToLowerInvariant() switch
            {
                "fullname" => ApplyOrder(query, ordered, s => s.FullName, descending),
                "email" => ApplyOrder(query, ordered, s => s.Email, descending),
                "dateofbirth" => ApplyOrder(query, ordered, s => s.DateOfBirth, descending),
                _ => ordered
            };

            if (ordered is not null)
            {
                query = ordered;
            }
        }

        return ordered ?? query;
    }

    private static IOrderedQueryable<Student> ApplyOrder<TKey>(
        IQueryable<Student> source,
        IOrderedQueryable<Student>? ordered,
        System.Linq.Expressions.Expression<Func<Student, TKey>> keySelector,
        bool descending)
    {
        if (ordered is null)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        return descending ? ordered.ThenByDescending(keySelector) : ordered.ThenBy(keySelector);
    }

    private static StudentModel MapToModel(Student student)
    {
        return new StudentModel
        {
            StudentId = student.StudentId,
            FullName = student.FullName,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth,
            Enrollments = student.Enrollments.Count == 0
                ? null
                : student.Enrollments.Select(e => new EnrollmentModel
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseId = e.CourseId,
                    Course = e.Course is null ? null : new CourseModel
                    {
                        CourseId = e.Course.CourseId,
                        CourseName = e.Course.CourseName,
                        SemesterId = e.Course.SemesterId,
                        SubjectId = e.Course.SubjectId
                    },
                    EnrollDate = e.EnrollDate,
                    Status = e.Status
                }).ToList()
        };
    }
}
