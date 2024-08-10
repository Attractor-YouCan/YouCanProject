using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository.Repository;

public class OrtTestRepository : IRepository<OrtTest>
{
    private readonly YouCanContext _context;
    private DbSet<OrtTest> entitis;


    public OrtTestRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<OrtTest>();
    }
    public IEnumerable<OrtTest> GetAll()
    {
        return entitis
            .Include(t => t.Tests)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .Include(t => t.Tests)
            .ThenInclude(t => t.Subject)
            .Include(t => t.Tests)
                .ThenInclude(s => s.OrtInstruction)
            .AsEnumerable();
        
    }

    public async Task<OrtTest> Get(int id)
    {
        return (await entitis
            .Include(t => t.Tests)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .Include(t => t.Tests)
                .ThenInclude(t => t.Subject)
            .Include(t => t.Tests)
                .ThenInclude(s => s.OrtInstruction)
            .SingleOrDefaultAsync(t => t.Id == id))!;
    }

    public async Task Insert(OrtTest entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(OrtTest entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(OrtTest entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(OrtTest entity)
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