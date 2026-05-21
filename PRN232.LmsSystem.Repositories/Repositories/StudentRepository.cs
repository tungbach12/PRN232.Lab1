using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Data;
using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly LmsDbContext _context;

    public StudentRepository(LmsDbContext context)
    {
        _context = context;
    }

    public IQueryable<Student> Query(bool includeEnrollments)
    {
        var query = _context.Students.AsNoTracking();
        if (includeEnrollments)
        {
            query = query
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course);
        }

        return query;
    }

    public async Task<Student?> GetByIdAsync(int id, bool includeEnrollments)
    {
        var query = Query(includeEnrollments);
        return await query.FirstOrDefaultAsync(s => s.StudentId == id);
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.AsNoTracking().ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task<Student> AddAsync(Student entity)
    {
        _context.Students.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Student entity)
    {
        var existing = await _context.Students.FindAsync(id);
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
        var existing = await _context.Students.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Students.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
