using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public interface ISubjectRepository
{
    IQueryable<Subject> Query(bool includeCourses);
    Task<Subject?> GetByIdAsync(int id, bool includeCourses);
    Task<Subject> AddAsync(Subject entity);
    Task<bool> UpdateAsync(int id, Subject entity);
    Task<bool> DeleteAsync(int id);
}
