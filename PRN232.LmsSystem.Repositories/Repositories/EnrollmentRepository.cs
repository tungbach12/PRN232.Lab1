using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Data;
using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly LmsDbContext _context;

    public EnrollmentRepository(LmsDbContext context)
    {
        _context = context;
    }

    public IQueryable<Enrollment> Query(bool includeStudent, bool includeCourse)
    {
        var query = _context.Enrollments.AsNoTracking();
        if (includeStudent)
        {
            query = query.Include(e => e.Student);
        }
        if (includeCourse)
        {
            query = query.Include(e => e.Course);
        }
        return query;
    }

    public async Task<Enrollment?> GetByIdAsync(int id, bool includeStudent, bool includeCourse)
    {
        var query = Query(includeStudent, includeCourse);
        return await query.FirstOrDefaultAsync(e => e.EnrollmentId == id);
    }

    public async Task<Enrollment> AddAsync(Enrollment entity)
    {
        _context.Enrollments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Enrollment entity)
    {
        var existing = await _context.Enrollments.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        // Set properties
        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Enrollments.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Enrollments.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
