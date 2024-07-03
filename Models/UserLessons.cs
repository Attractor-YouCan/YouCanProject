namespace YouCan.Models;

public class UserLessons
{
    public int Id { get; set; }
    public bool IsPassed { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    public int? SubtopicId { get; set; }
    public Subtopic? Subtopic { get; set; }
    public int? PassedLevel { get; set; }
    public int? LessonId { get; set; }
    public Lesson? Lesson { get; set; }
}
