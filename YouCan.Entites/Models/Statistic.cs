namespace YouCan.Entities;

public class Statistic : EntityBase
{
    public int Id { get; set; }
    public int? Streak { get; set; }
    public TimeSpan StudyMinutes { get; set; }
}
