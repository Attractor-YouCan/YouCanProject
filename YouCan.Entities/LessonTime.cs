namespace YouCan.Entities;

public class LessonTime : EntityBase
{
    public int UserId { get; set; }
    public User? User { get; set; }
    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public TimeSpan TimeSpent { get; set; }
    public DateOnly Date { get; set; }
}
