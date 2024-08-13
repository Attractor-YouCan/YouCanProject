using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YouCan.Repository;

namespace YouCan.Service.BackgroundServices;

public class LeagueUpdateService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

    public LeagueUpdateService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<YouCanContext>();
                await UpdateLeaguesAsync(context);
            }
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task UpdateLeaguesAsync(YouCanContext context)
    {
        var now = DateTime.UtcNow;
        var leagues = await context.Leagues.Where(l => l.EndDate <= now).ToListAsync();
        
        foreach (var league in leagues)
        {
            // Logic to reset or update the league
            league.StartDate = now;
            league.EndDate = now.AddMonths(1); // For example, reset to next month

            // Recalculate ranks or other logic here
        }

        await context.SaveChangesAsync();
    }
}