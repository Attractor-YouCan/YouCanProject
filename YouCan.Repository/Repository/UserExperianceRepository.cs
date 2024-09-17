using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouCan.Repository.Repository;
public class UserExperianceRepository : IRepository<UserExperience>
{
    private readonly YouCanContext _context;
    private DbSet<UserExperience> entitis;

    public UserExperianceRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<UserExperience>();
    }

    public IEnumerable<UserExperience> GetAll()
    {
        return entitis.AsEnumerable();
    }
    public async Task<UserExperience> Get(int id)
    {
        return await entitis.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task Insert(UserExperience entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(UserExperience entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(UserExperience entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(UserExperience entity)
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

