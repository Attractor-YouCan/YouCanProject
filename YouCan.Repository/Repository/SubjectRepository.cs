using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository;

public class SubjectRepository : IRepository<Subject>
{
    private readonly YouCanContext _context;
    private DbSet<Subject> entitis;


    public SubjectRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<Subject>();
    }
    public IEnumerable<Subject> GetAll()
    {
        return entitis
            .Include(s => s.Lessons)
                .ThenInclude(l => l.Tests)
                    .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
            .Include(s => s.Parent)
            .Include(s => s.SubSubjects)
            .Include(s => s.Lessons)
                .ThenInclude(l => l.LessonModules)
            .AsEnumerable();
    }

    public async Task<Subject> Get(int id)
    {
        return await entitis
            .Include(s => s.Lessons)
            .ThenInclude(l => l.Tests)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .Include(s => s.Parent)
            .Include(s => s.SubSubjects)
            .Include(s => s.Lessons)
            .ThenInclude(l => l.LessonModules)
            .SingleOrDefaultAsync(l => l.Id == id);
    }

    public async Task Insert(Subject entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Subject entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Subject entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Subject entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}