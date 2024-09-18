using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Repository;

public class UserOrtTestRepository : IRepository<UserOrtTest>
{
    private readonly YouCanContext _context;
    private DbSet<UserOrtTest> entitis;


    public UserOrtTestRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<UserOrtTest>();
    }
    public IEnumerable<UserOrtTest> GetAll()
    {
        return entitis
            .Include(ut => ut.User)
            .Include(ut => ut.OrtTest)
            .AsEnumerable();
    }

    public async Task<UserOrtTest> Get(int id)
    {
        return (await entitis
            .Include(ut => ut.User)
            .Include(ut => ut.OrtTest)
            .SingleOrDefaultAsync(e => e.Id == id))!;
    }

    public async Task Insert(UserOrtTest entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(UserOrtTest entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(UserOrtTest entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(UserOrtTest entity)
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