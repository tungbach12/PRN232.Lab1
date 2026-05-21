using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _repository;

    public SubjectService(ISubjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<SubjectModel>> GetSubjectsAsync(SubjectQueryModel query)
    {
        var subjectsQuery = _repository.Query(query.IncludeCourses);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            subjectsQuery = subjectsQuery.Where(s => s.SubjectName.Contains(keyword) || s.SubjectCode.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            subjectsQuery = ApplySorting(subjectsQuery, query.Sort);
        }

        var totalItems = await subjectsQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var subjects = await subjectsQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<SubjectModel>
        {
            Items = subjects.Select(MapToModel).ToList(),
            TotalItems = totalItems,
            Page = page,
            PageSize = size,
            TotalPages = totalPages
        };
    }

    public async Task<SubjectModel?> GetSubjectByIdAsync(int id, bool includeCourses)
    {
        var subject = await _repository.GetByIdAsync(id, includeCourses);
        return subject is null ? null : MapToModel(subject);
    }

    public async Task<SubjectModel> CreateSubjectAsync(SubjectModel model)
    {
        var entity = new Subject
        {
            SubjectCode = model.SubjectCode,
            SubjectName = model.SubjectName,
            Credit = model.Credit
        };

        var created = await _repository.AddAsync(entity);
        return MapToModel(created);
    }

    public async Task<bool> UpdateSubjectAsync(int id, SubjectModel model)
    {
        var entity = new Subject
        {
            SubjectId = id,
            SubjectCode = model.SubjectCode,
            SubjectName = model.SubjectName,
            Credit = model.Credit
        };

        return await _repository.UpdateAsync(id, entity);
    }

    public Task<bool> DeleteSubjectAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static IQueryable<Subject> ApplySorting(IQueryable<Subject> query, string sort)
    {
        var sortFields = sort.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        IOrderedQueryable<Subject>? ordered = null;

        foreach (var field in sortFields)
        {
            var descending = field.StartsWith('-');
            var name = descending ? field[1..] : field;

            ordered = name.ToLowerInvariant() switch
            {
                "subjectcode" => ApplyOrder(query, ordered, s => s.SubjectCode, descending),
                "subjectname" => ApplyOrder(query, ordered, s => s.SubjectName, descending),
                "credit" => ApplyOrder(query, ordered, s => s.Credit, descending),
                _ => ordered
            };

            if (ordered is not null)
            {
                query = ordered;
            }
        }

        return ordered ?? query;
    }

    private static IOrderedQueryable<Subject> ApplyOrder<TKey>(
        IQueryable<Subject> source,
        IOrderedQueryable<Subject>? ordered,
        System.Linq.Expressions.Expression<Func<Subject, TKey>> keySelector,
        bool descending)
    {
        if (ordered is null)
        {
            return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
        }

        return descending ? ordered.ThenByDescending(keySelector) : ordered.ThenBy(keySelector);
    }

    private static SubjectModel MapToModel(Subject subject)
    {
        return new SubjectModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit,
            Courses = subject.Courses.Count == 0
                ? null
                : subject.Courses.Select(c => new CourseModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    SemesterId = c.SemesterId,
                    SubjectId = c.SubjectId
                }).ToList()
        };
    }
}
