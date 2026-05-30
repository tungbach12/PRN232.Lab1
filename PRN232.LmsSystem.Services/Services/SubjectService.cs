using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Extensions;
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
        var subjectsQuery = _repository.Query();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            subjectsQuery = subjectsQuery.Where(s => s.SubjectName.Contains(keyword) || s.SubjectCode.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            subjectsQuery = subjectsQuery.ApplySort(query.Sort);
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

    public async Task<SubjectModel?> GetSubjectByIdAsync(int id)
    {
        var subject = await _repository.GetByIdAsync(id);
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



    private static SubjectModel MapToModel(Subject subject)
    {
        return new SubjectModel
        {
            SubjectId = subject.SubjectId,
            SubjectCode = subject.SubjectCode,
            SubjectName = subject.SubjectName,
            Credit = subject.Credit
        };
    }
}
