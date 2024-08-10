using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Repository;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly YouCanContext _context;
    private DbSet<T> entitis;


    public Repository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        return entitis.AsEnumerable();
    }

    public async Task<T> Get(int id)
    {
        return await entitis.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task Insert(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(T entity)
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