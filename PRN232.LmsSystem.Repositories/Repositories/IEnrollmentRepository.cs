using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public interface IEnrollmentRepository
{
    IQueryable<Enrollment> Query(bool includeStudent, bool includeCourse);
    Task<Enrollment?> GetByIdAsync(int id, bool includeStudent, bool includeCourse);
    Task<Enrollment> AddAsync(Enrollment entity);
    Task<bool> UpdateAsync(int id, Enrollment entity);
    Task<bool> DeleteAsync(int id);
}
