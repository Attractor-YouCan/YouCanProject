using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouCan.Repository;

public class UserRepository<T> : IRepository<T> where T : IdentityUser<int>
{
    private readonly YouCanContext _context;
    private DbSet<T> entitis;


    public UserRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        return entitis.Include("Statistic").Include("UserExperiences").AsEnumerable();
    }

    public async Task<T> Get(int id)
    {
        return await entitis.Include("Statistic").Include("UserExperiences").Include("League").SingleOrDefaultAsync(x => x.Id == id);
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