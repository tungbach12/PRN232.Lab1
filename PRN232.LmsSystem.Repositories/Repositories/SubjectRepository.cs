using Microsoft.EntityFrameworkCore;
using PRN232.LmsSystem.Repositories.Data;
using PRN232.LmsSystem.Repositories.Entities;

namespace PRN232.LmsSystem.Repositories.Repositories;

public class SubjectRepository : ISubjectRepository
{
    private readonly LmsDbContext _context;

    public SubjectRepository(LmsDbContext context)
    {
        _context = context;
    }

    public IQueryable<Subject> Query()
    {
        return _context.Subjects.AsNoTracking();
    }

    public async Task<Subject?> GetByIdAsync(int id)
    {
        var query = Query();
        return await query.FirstOrDefaultAsync(s => s.SubjectId == id);
    }

    public async Task<Subject> AddAsync(Subject entity)
    {
        _context.Subjects.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(int id, Subject entity)
    {
        var existing = await _context.Subjects.FindAsync(id);
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
        var existing = await _context.Subjects.FindAsync(id);
        if (existing is null)
        {
            return false;
        }

        _context.Subjects.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
