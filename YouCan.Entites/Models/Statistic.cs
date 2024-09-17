namespace YouCan.Entities;

public class Statistic : EntityBase
{
    public int Id { get; set; }
    public int? Streak { get; set; }
    public TimeSpan StudyMinutes { get; set; } = TimeSpan.Zero;
    public DateTime? ImpactModeStart { get; set; }
    public DateTime? ImpactModeEnd { get; set; }
    public DateTime? LastStudyDate { get; set; }
    public int ImpactModeDays
    {
        get
        {
            if (ImpactModeStart.HasValue && ImpactModeEnd.HasValue)
            {
                return (ImpactModeEnd.Value - ImpactModeStart.Value).Days + 1;
            }
            return 0;
        }
    }
}
