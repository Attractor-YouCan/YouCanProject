using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouCan.Entities;

namespace YouCan.Repository;

public class AnnouncementRepository : IRepository<Announcement>
{
    private readonly YouCanContext _context;
    private readonly DbSet<Announcement> entities;

    public AnnouncementRepository(YouCanContext context)
    {
        _context = context;
        entities = _context.Set<Announcement>();
    }
    public async Task Delete(Announcement entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<Announcement> Get(int id) => entities.FirstOrDefaultAsync(e => e.Id == id);

    public IEnumerable<Announcement> GetAll() => entities.AsEnumerable();

    public async Task Insert(Announcement entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        await entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Announcement entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }
        entities.Remove(entity);
        _context.SaveChanges();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Update(Announcement entity)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }
        entities.Update(entity);
        await _context.SaveChangesAsync();
    }
}
