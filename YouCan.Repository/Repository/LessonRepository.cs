using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Repository;

public class LessonRepository : IRepository<Lesson>
{
    private readonly YouCanContext _context;
    private DbSet<Lesson> entitis;


    public LessonRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<Lesson>();
    }
    public IEnumerable<Lesson> GetAll()
    {
        return entitis
            .Include(l => l.Subject)
                .ThenInclude(s => s.Lessons)
                    .ThenInclude(s => s.Subject)
            .Include(l => l.Tests)
                .ThenInclude(t => t.Subject)
            .Include(l => l.Tests)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .Include(l => l.LessonModules)
            .AsEnumerable();
    }

    public async Task<Lesson> Get(int id)
    {
        return await entitis
            .Include(l => l.Subject)
                .ThenInclude(s => s.Lessons)
                    .ThenInclude(s => s.Subject)
            .Include(l => l.Tests)
                .ThenInclude(t => t.Subject)
            .Include(l => l.Tests)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .Include(l => l.LessonModules)
            .SingleOrDefaultAsync(l => l.Id == id);
    }

    public async Task Insert(Lesson entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Lesson entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Lesson entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Lesson entity)
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