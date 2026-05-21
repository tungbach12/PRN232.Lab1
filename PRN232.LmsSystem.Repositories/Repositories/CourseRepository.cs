using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Data;
using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly LmsDbContext _context;

    public CourseRepository(LmsDbContext context)
    {
        _context = context;
    }

    public IQueryable<Course> Query(bool includeSemester, bool includeSubject, bool includeEnrollments)
    {
        var query = _context.Courses.AsNoTracking();
        if (includeSemester)
        {
            query = query.Include(c => c.Semester);
        }

        if (includeSubject)
        {
            query = query.Include(c => c.Subject);
        }

        if (includeEnrollments)
        {
            query = query
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student);
        }

        return query;
    }

    public async Task<Course?> GetByIdAsync(int id, bool includeSemester, bool includeSubject, bool includeEnrollments)
    {
        var query = Query(includeSemester, includeSubject, includeEnrollments);
        return await query.FirstOrDefaultAsync(c => c.CourseId == id);
    }

    public async Task<Course> AddAsync(Course entity)
    {
        _context.Courses.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Course entity)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Courses.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Courses.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
