using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using YouCan.Entities;

namespace YouCan.Services;

public class TariffCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    public TariffCheckService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var users = await userManager.Users.Where(u => u.TariffEndDate <= DateTime.UtcNow).ToListAsync();

                foreach (var user in users)
                {
                    user.TariffId = 1; // Тариф Start
                    user.TariffEndDate = null;

                    if (await userManager.IsInRoleAsync(user, "prouser"))
                    {
                        await userManager.AddToRoleAsync(user, "user");
                        await userManager.RemoveFromRoleAsync(user, "prouser");
                    }
                    await userManager.UpdateAsync(user);
                }
            }
            await Task.Delay(TimeSpan.FromDays(1), token);
            // Проверка пользователей на истечение тарифа проходит раз в день
        }
    }
}