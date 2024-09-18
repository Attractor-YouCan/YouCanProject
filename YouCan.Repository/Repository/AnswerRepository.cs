using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository;

public class AnswerRepository : IRepository<Answer>
{
    private readonly YouCanContext _context;
    private DbSet<Answer> entitis;


    public AnswerRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<Answer>();
    }
    public IEnumerable<Answer> GetAll()
    {
        return entitis
            .Include(a => a.Question)
            .AsEnumerable();
    }

    public async Task<Answer> Get(int id)
    {
        return (await entitis
            .Include(a => a.Question)
            .SingleOrDefaultAsync(a => a.Id == id))!;
    }

    public async Task Insert(Answer entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Answer entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Answer entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Answer entity)
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