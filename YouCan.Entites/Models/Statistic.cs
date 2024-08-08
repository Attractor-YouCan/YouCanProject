namespace YouCan.Entities;

public class Statistic : EntityBase
{
    public int Streak { get; set; }
    public int TotalExperience { get; set; }
    public TimeSpan StudyMinutes { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
