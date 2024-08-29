using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouCan.Entites.Models;

namespace YouCan.Repository.Repository;

public class TariffRepository : IRepository<Tariff>
{
    private readonly YouCanContext _context;
    private DbSet<Tariff> entitis;


    public TariffRepository(YouCanContext context)
    {
        _context = context;
        entitis = _context.Set<Tariff>();
    }
    public IEnumerable<Tariff> GetAll()
    {
        return entitis
            .Include(t => t.Users)
            .AsEnumerable();
    }

    public async Task<Tariff> Get(int id)
    {
        return (await entitis
            .SingleOrDefaultAsync(a => a.Id == id))!;
    }

    public async Task Insert(Tariff entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Tariff entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Tariff entity)
    {
        if (entity == null)
            throw new ArgumentNullException("entity");
        entitis.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public void Remove(Tariff entity)
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