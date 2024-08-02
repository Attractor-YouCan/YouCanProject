using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Repository;

public class UserLessonsRepository : IRepository<UserLessons>
{
    private readonly YouCanContext _context;
    private DbSet<UserLessons> entitis;


    public UserLessonsRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<UserLessons>();
    }
    public IEnumerable<UserLessons> GetAll()
    {
        return entitis
            .Include(ul => ul.Lesson)
                .ThenInclude(l => l.Subject)
            .Include(ul => ul.Subject)
            .Include(ul => ul.User)
            .AsEnumerable();
    }

    public async Task<UserLessons> Get(int id)
    {
        return await entitis
            .Include(ul => ul.Lesson)
            .ThenInclude(l => l.Subject)
            .Include(ul => ul.Subject)
            .Include(ul => ul.User)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task Insert(UserLessons entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(UserLessons entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(UserLessons entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(UserLessons entity)
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