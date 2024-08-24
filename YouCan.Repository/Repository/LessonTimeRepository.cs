using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouCan.Entities;

namespace YouCan.Repository;

public class LessonTimeRepository : IRepository<LessonTime>
{
    private readonly YouCanContext _context;
    private DbSet<LessonTime> entities;
    public LessonTimeRepository(YouCanContext context)
    {
        _context = context;
        entities = _context.Set<LessonTime>();
    }
    public IEnumerable<LessonTime> GetAll()
    {
        return entities
            .Include(l => l.User)
            .Include(l => l.Lesson)
            .AsEnumerable();
    }

    public async Task<LessonTime?> Get(int id)
    {
        return await entities
            .Include(l => l.User)
            .Include(l => l.Lesson)
            .SingleOrDefaultAsync(l => l.Id == id);
    }

    public async Task Insert(LessonTime entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entities.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(LessonTime entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(LessonTime entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(LessonTime entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entities.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
