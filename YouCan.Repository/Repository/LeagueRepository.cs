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

        public async Task CreateLeagueAsync(string leagueName, int minPoints, int maxPoints, DateTime startDate, DateTime endDate)
        {
            var league = new League
            {
                LeagueName = leagueName,
                MinPoints = minPoints,
                MaxPoints = maxPoints,
                StartDate = startDate,
                EndDate = endDate
            };

            _entities.Add(league);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeaguesAsync()
        {
            var now = DateTime.UtcNow;
            var leagues = await _entities.Where(l => l.EndDate <= now).ToListAsync();

            foreach (var league in leagues)
            {
                // Logic to reset or update the league
                league.StartDate = now;
                league.EndDate = now.AddMonths(1); // For example, reset to next month

                // Recalculate ranks or other logic here
            }

            await _context.SaveChangesAsync();
        }
    }
}
