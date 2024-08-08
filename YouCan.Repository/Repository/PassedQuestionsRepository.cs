using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Repository;

public class PassedQuestionsRepository : IRepository<PassedQuestion>
{
    private readonly YouCanContext _context;
    private DbSet<PassedQuestion> entitis;


    public PassedQuestionsRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<PassedQuestion>();
    }
    public IEnumerable<PassedQuestion> GetAll()
    {
        return entitis
            .Include(ut => ut.User)
            .Include(ut => ut.Question)
            .AsEnumerable();
    }

    public async Task<PassedQuestion> Get(int id)
    {
        return (await entitis
            .Include(ut => ut.User)
            .Include(ut => ut.Question)
            .SingleOrDefaultAsync(e => e.Id == id))!;
    }

    public async Task Insert(PassedQuestion entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(PassedQuestion entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(PassedQuestion entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(PassedQuestion entity)
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