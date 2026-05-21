using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public interface ISemesterRepository
{
    IQueryable<Semester> Query(bool includeCourses);
    Task<Semester?> GetByIdAsync(int id, bool includeCourses);
    Task<Semester> AddAsync(Semester entity);
    Task<bool> UpdateAsync(int id, Semester entity);
    Task<bool> DeleteAsync(int id);
}
