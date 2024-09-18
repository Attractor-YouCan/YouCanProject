using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository;

public class QuestionReportRepository : IRepository<QuestionReport>
{
    private readonly YouCanContext _context;
    private DbSet<QuestionReport> entitis;


    public QuestionReportRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<QuestionReport>();
    }
    public IEnumerable<QuestionReport> GetAll()
    {
        return entitis
            .Include(ut => ut.User)
            .Include(ut => ut.Question)
            .AsEnumerable();
    }

    public async Task<QuestionReport> Get(int id)
    {
        return (await entitis
            .Include(ut => ut.User)
            .Include(ut => ut.Question)
            .SingleOrDefaultAsync(e => e.Id == id))!;
    }

    public async Task Insert(QuestionReport entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(QuestionReport entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(QuestionReport entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(QuestionReport entity)
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