using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public class SemesterService : ISemesterService
{
    private readonly ISemesterRepository _repository;

    public SemesterService(ISemesterRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<SemesterModel>> GetSemestersAsync(SemesterQueryModel query)
    {
        var semestersQuery = _repository.Query(query.IncludeCourses);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            semestersQuery = semestersQuery.Where(s => s.SemesterName.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            semestersQuery = ApplySorting(semestersQuery, query.Sort);
        }

        var totalItems = await semestersQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var semesters = await semestersQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<SemesterModel>
        {
            Items = semesters.Select(MapToModel).ToList(),
            TotalItems = totalItems,
            Page = page,
            PageSize = size,
            TotalPages = totalPages
        };
    }

    public async Task<SemesterModel?> GetSemesterByIdAsync(int id, bool includeCourses)
    {
        var semester = await _repository.GetByIdAsync(id, includeCourses);
        return semester is null ? null : MapToModel(semester);
    }

    public async Task<SemesterModel> CreateSemesterAsync(SemesterModel model)
    {
        var entity = new Semester
        {
            SemesterName = model.SemesterName,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

        var created = await _repository.AddAsync(entity);
        return MapToModel(created);
    }

    public async Task<bool> UpdateSemesterAsync(int id, SemesterModel model)
    {
        var entity = new Semester
        {
            SemesterId = id,
            SemesterName = model.SemesterName,
            StartDate = model.StartDate,
            EndDate = model.EndDate
        };

        return await _repository.UpdateAsync(id, entity);
    }

    public Task<bool> DeleteSemesterAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static IQueryable<Semester> ApplySorting(IQueryable<Semester> query, string sort)
    {
        var sortFields = sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IOrderedQueryable<Semester>? ordered = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith('-');
            var name = descending ? field[1..] : field;

            ordered = name.ToLowerInvariant() switch
            {
                "semestername" => ApplyOrder(query, ordered, s => s.SemesterName, descending),
                "startdate" => ApplyOrder(query, ordered, s => s.StartDate, descending),
                "enddate" => ApplyOrder(query, ordered, s => s.EndDate, descending),
                _ => ordered
            };

            if (ordered is not null)
            {
                query = ordered;
            }
        }

        return ordered ?? query;
    }

    private static IOrderedQueryable<Semester> ApplyOrder<TKey>(
        IQueryable<Semester> source,
        IOrderedQueryable<Semester>? ordered,
        System.Linq.Expressions.Expression<Func<Semester, TKey>> keySelector,
        bool descending)
    {
        if (ordered is null)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        return descending ? ordered.ThenByDescending(keySelector) : ordered.ThenBy(keySelector);
    }

    private static SemesterModel MapToModel(Semester semester)
    {
        return new SemesterModel
        {
            SemesterId = semester.SemesterId,
            SemesterName = semester.SemesterName,
            StartDate = semester.StartDate,
            EndDate = semester.EndDate,
            Courses = semester.Courses.Count == 0
                ? null
                : semester.Courses.Select(c => new CourseModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    SemesterId = c.SemesterId,
                    SubjectId = c.SubjectId
                }).ToList()
        };
    }
}
