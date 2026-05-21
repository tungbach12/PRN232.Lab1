using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Data;
using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public class SemesterRepository : ISemesterRepository
{
    private readonly LmsDbContext _context;

    public SemesterRepository(LmsDbContext context)
    {
        _context = context;
    }

    public IQueryable<Semester> Query(bool includeCourses)
    {
        var query = _context.Semesters.AsNoTracking();
        if (includeCourses)
        {
            query = query.Include(s => s.Courses);
        }

        return query;
    }

    public async Task<Semester?> GetByIdAsync(int id, bool includeCourses)
    {
        var query = Query(includeCourses);
        return await query.FirstOrDefaultAsync(s => s.SemesterId == id);
    }

    public async Task<Semester> AddAsync(Semester entity)
    {
        _context.Semesters.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Semester entity)
    {
        var existing = await _context.Semesters.FindAsync(id);
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
        var existing = await _context.Semesters.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Semesters.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
