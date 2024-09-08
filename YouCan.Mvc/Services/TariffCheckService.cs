using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;

namespace YouCan.Mvc.Services;

public class TariffCheckService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TariffCheckService> _logger; // Добавляем логгер

    public TariffCheckService(IServiceProvider serviceProvider, ILogger<TariffCheckService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger; // Инициализируем логгер
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        _logger.LogInformation("TariffCheckService started.");

        try
        {
            while (!token.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var users = await userManager.Users.Where(u => u.TariffEndDate <= DateTime.UtcNow).ToListAsync(token); // Указываем токен

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

                await Task.Delay(TimeSpan.FromDays(1), token); // Указываем токен для отмены
            }
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("TariffCheckService was canceled."); // Логируем отмену
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in TariffCheckService."); // Логируем любые другие ошибки
        }
        finally
        {
            _logger.LogInformation("TariffCheckService is stopping.");
        }
    }
}
