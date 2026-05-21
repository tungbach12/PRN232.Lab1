using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public interface ICourseService
{
    Task<PagedResult<CourseModel>> GetCoursesAsync(CourseQueryModel query);
    Task<CourseModel?> GetCourseByIdAsync(int id, bool includeSemester, bool includeSubject, bool includeEnrollments);
    Task<CourseModel> CreateCourseAsync(CourseModel model);
    Task<bool> UpdateCourseAsync(int id, CourseModel model);
    Task<bool> DeleteCourseAsync(int id);
}
