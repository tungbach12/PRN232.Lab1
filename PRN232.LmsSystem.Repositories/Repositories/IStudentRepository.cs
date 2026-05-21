using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public interface IStudentRepository
{
    IQueryable<Student> Query(bool includeEnrollments);
    Task<Student?> GetByIdAsync(int id, bool includeEnrollments);
    Task<Student> AddAsync(Student entity);
    Task<bool> UpdateAsync(int id, Student entity);
    Task<bool> DeleteAsync(int id);
}
