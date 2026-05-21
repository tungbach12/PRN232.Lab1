using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public interface IStudentService
{
    Task<PagedResult<StudentModel>> GetStudentsAsync(StudentQueryModel query);
    Task<StudentModel?> GetStudentByIdAsync(int id, bool includeEnrollments);
    Task<StudentModel> CreateStudentAsync(StudentModel model);
    Task<bool> UpdateStudentAsync(int id, StudentModel model);
    Task<bool> DeleteStudentAsync(int id);
}
