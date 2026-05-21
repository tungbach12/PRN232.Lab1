using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public interface ISemesterService
{
    Task<PagedResult<SemesterModel>> GetSemestersAsync(SemesterQueryModel query);
    Task<SemesterModel?> GetSemesterByIdAsync(int id, bool includeCourses);
    Task<SemesterModel> CreateSemesterAsync(SemesterModel model);
    Task<bool> UpdateSemesterAsync(int id, SemesterModel model);
    Task<bool> DeleteSemesterAsync(int id);
}
