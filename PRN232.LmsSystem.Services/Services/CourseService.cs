using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;

    public CourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<CourseModel>> GetCoursesAsync(CourseQueryModel query)
    {
        var coursesQuery = _repository.Query(query.IncludeSemester, query.IncludeSubject, query.IncludeEnrollments);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            coursesQuery = coursesQuery.Where(c => c.CourseName.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            coursesQuery = ApplySorting(coursesQuery, query.Sort);
        }

        var totalItems = await coursesQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var courses = await coursesQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<CourseModel>
        {
            Items = courses.Select(MapToModel).ToList(),
            TotalItems = totalItems,
            Page = page,
            PageSize = size,
            TotalPages = totalPages
        };
    }

    public async Task<CourseModel?> GetCourseByIdAsync(int id, bool includeSemester, bool includeSubject, bool includeEnrollments)
    {
        var course = await _repository.GetByIdAsync(id, includeSemester, includeSubject, includeEnrollments);
        return course is null ? null : MapToModel(course);
    }

    public async Task<CourseModel> CreateCourseAsync(CourseModel model)
    {
        var entity = new Course
        {
            CourseName = model.CourseName,
            SemesterId = model.SemesterId,
            SubjectId = model.SubjectId
        };

        var created = await _repository.AddAsync(entity);
        return MapToModel(created);
    }

    public async Task<bool> UpdateCourseAsync(int id, CourseModel model)
    {
        var entity = new Course
        {
            CourseId = id,
            CourseName = model.CourseName,
            SemesterId = model.SemesterId,
            SubjectId = model.SubjectId
        };

        return await _repository.UpdateAsync(id, entity);
    }

    public Task<bool> DeleteCourseAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static IQueryable<Course> ApplySorting(IQueryable<Course> query, string sort)
    {
        var sortFields = sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IOrderedQueryable<Course>? ordered = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith('-');
            var name = descending ? field[1..] : field;

            ordered = name.ToLowerInvariant() switch
            {
                "coursename" => ApplyOrder(query, ordered, c => c.CourseName, descending),
                "semesterid" => ApplyOrder(query, ordered, c => c.SemesterId, descending),
                "subjectid" => ApplyOrder(query, ordered, c => c.SubjectId, descending),
                _ => ordered
            };

            if (ordered is not null)
            {
                query = ordered;
            }
        }

        return ordered ?? query;
    }

    private static IOrderedQueryable<Course> ApplyOrder<TKey>(
        IQueryable<Course> source,
        IOrderedQueryable<Course>? ordered,
        System.Linq.Expressions.Expression<Func<Course, TKey>> keySelector,
        bool descending)
    {
        if (ordered is null)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        return descending ? ordered.ThenByDescending(keySelector) : ordered.ThenBy(keySelector);
    }

    private static CourseModel MapToModel(Course course)
    {
        return new CourseModel
        {
            CourseId = course.CourseId,
            CourseName = course.CourseName,
            SemesterId = course.SemesterId,
            SubjectId = course.SubjectId,
            Semester = course.Semester is null ? null : new SemesterModel
            {
                SemesterId = course.Semester.SemesterId,
                SemesterName = course.Semester.SemesterName,
                StartDate = course.Semester.StartDate,
                EndDate = course.Semester.EndDate
            },
            Subject = course.Subject is null ? null : new SubjectModel
            {
                SubjectId = course.Subject.SubjectId,
                SubjectCode = course.Subject.SubjectCode,
                SubjectName = course.Subject.SubjectName,
                Credit = course.Subject.Credit
            },
            Enrollments = course.Enrollments.Count == 0
                ? null
                : course.Enrollments.Select(e => new EnrollmentModel
                {
                    EnrollmentId = e.EnrollmentId,
                    StudentId = e.StudentId,
                    CourseId = e.CourseId,
                    EnrollDate = e.EnrollDate,
                    Status = e.Status,
                    Student = e.Student is null ? null : new StudentModel
                    {
                        StudentId = e.Student.StudentId,
                        FullName = e.Student.FullName,
                        Email = e.Student.Email,
                        DateOfBirth = e.Student.DateOfBirth
                    }
                }).ToList()
        };
    }
}
