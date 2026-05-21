using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public interface ICourseRepository
{
    IQueryable<Course> Query(bool includeSemester, bool includeSubject, bool includeEnrollments);
    Task<Course?> GetByIdAsync(int id, bool includeSemester, bool includeSubject, bool includeEnrollments);
    Task<Course> AddAsync(Course entity);
    Task<bool> UpdateAsync(int id, Course entity);
    Task<bool> DeleteAsync(int id);
}
