namespace YouCan.Models;

public class Statistic
{
    public int Id { get; set; }
    public int Streak { get; set; }
    public int TotalExperience { get; set; }
    public int StudyMinutes { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
