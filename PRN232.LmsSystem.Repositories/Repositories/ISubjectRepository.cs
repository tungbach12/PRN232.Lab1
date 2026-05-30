using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public interface ISubjectRepository
{
    IQueryable<Subject> Query();
    Task<Subject?> GetByIdAsync(int id);
    Task<Subject> AddAsync(Subject entity);
    Task<bool> UpdateAsync(int id, Subject entity);
    Task<bool> DeleteAsync(int id);
}
