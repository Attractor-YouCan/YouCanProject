using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository;

public class LessonModuleRepository : IRepository<LessonModule>
{
    private readonly YouCanContext _context;
    private DbSet<LessonModule> entitis;


    public LessonModuleRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<LessonModule>();
    }
    public IEnumerable<LessonModule> GetAll()
    {
        return entitis
            .Include(lm => lm.Lesson)
            .AsEnumerable();
    }

    public async Task<LessonModule> Get(int id)
    {
        return (await entitis
            .Include(lm => lm.Lesson)
            .SingleOrDefaultAsync(e => e.Id == id))!;
    }

    public async Task Insert(LessonModule entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(LessonModule entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(LessonModule entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(LessonModule entity)
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