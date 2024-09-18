using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouCan.Entities;

namespace YouCan.Repository;

public class RealOrtTestRepository : IRepository<RealOrtTest>
{
    private readonly YouCanContext _context;
    private readonly DbSet<RealOrtTest> entities;
    public RealOrtTestRepository(YouCanContext context)
    {
        _context = context;
        entities = _context.Set<RealOrtTest>();
    }
    public async Task Delete(RealOrtTest entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<RealOrtTest> Get(int id)
    {
        var entity = await entities.FirstOrDefaultAsync(x => x.Id == id);
        if (entity != null)
        {
            return entity;
        }
        throw new ArgumentNullException(nameof(entity));
    }

    public IEnumerable<RealOrtTest> GetAll() => entities.AsEnumerable();

    public async Task Insert(RealOrtTest entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException();
        }
        await entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(RealOrtTest entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException();
        }
        entities.Remove(entity);
        _context.SaveChanges();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task Update(RealOrtTest entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException();
        }
        entities.Update(entity);
        await _context.SaveChangesAsync();
    }
}
