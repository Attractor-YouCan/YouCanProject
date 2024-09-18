using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository;

public class QuestionRepository : IRepository<Question>
{
    private readonly YouCanContext _context;
    private DbSet<Question> entitis;


    public QuestionRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<Question>();
    }
    public IEnumerable<Question> GetAll()
    {
        return entitis
            .Include(q => q.Answers)
            .Include(q => q.Test)
                .ThenInclude(t => t.Subject)
            .Include(q => q.User)
            .AsEnumerable();
    }

    public async Task<Question> Get(int id)
    {
        return await entitis
            .Include(q => q.Answers)
            .Include(q => q.Test)
            .ThenInclude(t => t.Subject)
            .Include(q => q.User)
            .Include(q => q.Subject)
            .SingleOrDefaultAsync(q => q.Id == id);
    }

    public async Task Insert(Question entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Question entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Question entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Question entity)
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