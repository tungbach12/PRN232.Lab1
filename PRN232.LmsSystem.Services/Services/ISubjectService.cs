using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public interface ISubjectService
{
    Task<PagedResult<SubjectModel>> GetSubjectsAsync(SubjectQueryModel query);
    Task<SubjectModel?> GetSubjectByIdAsync(int id);
    Task<SubjectModel> CreateSubjectAsync(SubjectModel model);
    Task<bool> UpdateSubjectAsync(int id, SubjectModel model);
    Task<bool> DeleteSubjectAsync(int id);
}
