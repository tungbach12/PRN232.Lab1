using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Entities;
using PRN232.LmsSystem.Repositories.Repositories;
using PRN232.LmsSystem.Services.Extensions;
using PRN232.LmsSystem.Services.Models;

namespace PRN232.LmsSystem.Services.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public StudentService(IStudentRepository repository, IEnrollmentRepository enrollmentRepository)
    {
        _repository = repository;
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<PagedResult<EnrollmentModel>> GetEnrollmentByStudentId(int Id, EnrollmentQueryModel query)
    {
        var includeStudent = query.IncludeStudent;
        var includeCourse = query.IncludeCourse;
        var enrollmentsQuery = _enrollmentRepository.Query(includeStudent, includeCourse)
                                                    .Where(s => s.StudentId == Id);
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            enrollmentsQuery = enrollmentsQuery.Where(e =>
                e.Status.Contains(keyword) ||
                (e.Student != null && e.Student.FullName.Contains(keyword)) ||
                (e.Course != null && e.Course.CourseName.Contains(keyword)));
        }
        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            enrollmentsQuery = enrollmentsQuery.ApplySort(query.Sort);
        }
        var totalItems = await enrollmentsQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);
        var enrollments = await enrollmentsQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        return new PagedResult<EnrollmentModel>
        {
            Items = enrollments.Select(MapToEnrollmentModel).ToList(),
            Page = page,
            PageSize = size,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }
    public async Task<PagedResult<StudentModel>> GetStudentsAsync(StudentQueryModel query)
    {
        var includeEnrollments = query.IncludeEnrollments;
        var studentsQuery = _repository.Query(includeEnrollments);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var keyword = query.Search.Trim();
            studentsQuery = studentsQuery.Where(s => s.FullName.Contains(keyword) || s.Email.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(query.Sort))
        {
            studentsQuery = studentsQuery.ApplySort(query.Sort);
        }

        var totalItems = await studentsQuery.CountAsync();
        var page = query.Page < 1 ? 1 : query.Page;
        var size = query.Size < 1 ? 10 : query.Size;
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var students = await studentsQuery
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new PagedResult<StudentModel>
        {
            Items = students.Select(MapToModel).ToList(),
            TotalItems = totalItems,
            Page = page,
            PageSize = size,
            TotalPages = totalPages
        };
    }

    public async Task<StudentModel?> GetStudentByIdAsync(int id, bool includeEnrollments)
    {
        var student = await _repository.GetByIdAsync(id, includeEnrollments);
        return student is null ? null : MapToModel(student);
    }

    public async Task<StudentModel> CreateStudentAsync(StudentModel model)
    {
        var entity = new Student
        {
            FullName = model.FullName,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth
        };

        var created = await _repository.AddAsync(entity);
        return MapToModel(created);
    }

    public async Task<bool> UpdateStudentAsync(int id, StudentModel model)
    {
        var entity = new Student
        {
            StudentId = id,
            FullName = model.FullName,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth
        };

        return await _repository.UpdateAsync(id, entity);
    }

    public Task<bool> DeleteStudentAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }



    private static StudentModel MapToModel(Student student)
    {
        return new StudentModel
        {
            StudentId = student.StudentId,
            FullName = student.FullName,
            Email = student.Email,
            DateOfBirth = student.DateOfBirth,
            Enrollments = student.Enrollments.Count == 0
                ? null
                : student.Enrollments.Select(e => new EnrollmentModel
                {
                    EnrollmentId = e.EnrollmentId,
                    CourseId = e.CourseId,
                    Course = e.Course is null ? null : new CourseModel
                    {
                        CourseId = e.Course.CourseId,
                        CourseName = e.Course.CourseName,
                        SemesterId = e.Course.SemesterId
                    },
                    EnrollDate = e.EnrollDate,
                    Status = e.Status
                }).ToList()
        };
    }

    private static EnrollmentModel MapToEnrollmentModel(Enrollment enrollment)
    {
        return new EnrollmentModel
        {
            EnrollmentId = enrollment.EnrollmentId,
            StudentId = enrollment.StudentId,
            CourseId = enrollment.CourseId,
            EnrollDate = enrollment.EnrollDate,
            Status = enrollment.Status,
            Student = enrollment.Student is null ? null : new StudentModel
            {
                StudentId = enrollment.Student.StudentId,
                FullName = enrollment.Student.FullName,
                Email = enrollment.Student.Email,
                DateOfBirth = enrollment.Student.DateOfBirth
            },
            Course = enrollment.Course is null ? null : new CourseModel
            {
                CourseId = enrollment.Course.CourseId,
                CourseName = enrollment.Course.CourseName,
                SemesterId = enrollment.Course.SemesterId
            }
        };
    }
}
