using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Repository.Repository;

namespace YouCan.Repository
{
    public class LeagueRepository : IRepository<League>
    {
        private readonly YouCanContext _context;
        private DbSet<League> _entities;

        public LeagueRepository(YouCanContext context)
        {
            _context = context;
            _entities = _context.Set<League>();
        }

        public IEnumerable<League> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<League> Get(int id)
        {
            return await _entities.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task Insert(League entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(League entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(League entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public void Remove(League entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Remove(entity);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CreateLeagueAsync(string leagueName, int minPoints, int maxPoints)
        {
            var league = new League
            {
                LeagueName = leagueName,
                MinPoints = minPoints,
                MaxPoints = maxPoints,
            };

            _entities.Add(league);
            await _context.SaveChangesAsync();
        }
        
    }
}
