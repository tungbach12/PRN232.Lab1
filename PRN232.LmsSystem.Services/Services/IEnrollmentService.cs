using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public interface IEnrollmentService
{
    Task<PagedResult<EnrollmentModel>> GetEnrollmentsAsync(EnrollmentQueryModel query);
    Task<EnrollmentModel?> GetEnrollmentByIdAsync(int id, bool includeStudent, bool includeCourse);
    Task<EnrollmentModel> CreateEnrollmentAsync(EnrollmentModel model);
    Task<bool> UpdateEnrollmentAsync(int id, EnrollmentModel model);
    Task<bool> DeleteEnrollmentAsync(int id);
}
