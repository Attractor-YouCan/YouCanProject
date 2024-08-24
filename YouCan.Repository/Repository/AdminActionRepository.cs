using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using YouCan.Entities;
using System.Linq;

namespace YouCan.Repository;

public class AdminActionRepository : IRepository<AdminAction>
{
    private readonly YouCanContext _context;
    private DbSet<AdminAction> entities;


    public AdminActionRepository(YouCanContext context)
    {
        _context = context;
        entities = _context.Set<AdminAction>();
    }
    public IEnumerable<AdminAction> GetAll()
    {
        return entities
            .Include(a => a.User)
            .AsEnumerable();
    }

    public async Task<AdminAction> Get(int id)
    {
        return (await entities
            .Include(a => a.User)
            .SingleOrDefaultAsync(a => a.Id == id))!;
    }

    public async Task Insert(AdminAction entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entities.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(AdminAction entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(AdminAction entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(AdminAction entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entities.Remove(entity);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
