using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Repository;

public class TestRepository : IRepository<Test>
{
    private readonly YouCanContext _context;
    private DbSet<Test> entitis;


    public TestRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<Test>();
    }
    public IEnumerable<Test> GetAll()
    {
        return entitis
            .Include(t => t.Subject)
            .Include(t => t.Lesson)
            .Include(t => t.OrtInstruction)
            .Include(t => t.OrtTest)
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .AsEnumerable();
    }

    public async Task<Test> Get(int id)
    {
        return (await entitis
            .Include(t => t.Subject)
            .Include(t => t.Lesson)
            .Include(t => t.OrtInstruction)
            .Include(t => t.OrtTest)
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .SingleOrDefaultAsync(e => e.Id == id))!;
    }

    public async Task Insert(Test entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Test entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Test entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Test entity)
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