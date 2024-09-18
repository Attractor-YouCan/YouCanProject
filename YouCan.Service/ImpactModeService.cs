using YouCan.Repository;

namespace YouCan.Service;

public class ImpactModeService : IImpactModeService
{
    private readonly YouCanContext _context;
    public ImpactModeService(YouCanContext context)
    {
        _context = context;
    }
    public async Task UpdateImpactMode(int statisticId)
    {
        var statistic = await _context.Statistics.FindAsync(statisticId);
        DateTime today = DateTime.UtcNow.Date;
        DateTime? lastStudyDate = statistic.LastStudyDate?.Date;
        if (lastStudyDate == null)
        {
            lastStudyDate = today;
            statistic.ImpactModeEnd = today;
            statistic.ImpactModeStart = today;
        }

        Console.WriteLine("UpdateImpactMode вызван");
        Console.WriteLine($"Сегодня: {today}, Последняя дата обучения: {lastStudyDate}");

        if (!statistic.ImpactModeStart.HasValue || !statistic.ImpactModeEnd.HasValue)
        {
            statistic.ImpactModeStart = today;
            statistic.ImpactModeEnd = today;
            Console.WriteLine($"Ударный режим установлен: Start={statistic.ImpactModeStart}, End={statistic.ImpactModeEnd}");

        }
        else
        {

            if (lastStudyDate.HasValue && lastStudyDate.Value == today.AddDays(-1))
            {
                statistic.ImpactModeEnd = today;
                Console.WriteLine($"Ударный режим продлен до {statistic.ImpactModeEnd}");
            }
            else if (lastStudyDate.HasValue && lastStudyDate.Value == today)
            {
                statistic.ImpactModeEnd = today;
                Console.WriteLine($"Ударный режим продлен до {statistic.ImpactModeEnd}");
            }
            else if (lastStudyDate.HasValue && lastStudyDate.Value < today.AddDays(-1))
            {
                statistic.ImpactModeEnd = today;
                statistic.ImpactModeStart = today;
                Console.WriteLine("Ударный режим сброшен из-за пропуска и установлен заново.");
            }
        }

        statistic.LastStudyDate = today;

        Console.WriteLine($"Обновленные данные: Start={statistic.ImpactModeStart}, End={statistic.ImpactModeEnd}, LastStudyDate={statistic.LastStudyDate}");

        try
        {
            _context.Statistics.Update(statistic);
            await _context.SaveChangesAsync();
            Console.WriteLine("Статистика обновлена");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении статистики: {ex.Message}");
        }
    }

}
