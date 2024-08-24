namespace YouCan.Entities;

public class LessonModule : EntityBase
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? PhotoUrl { get; set; }
    public int? LessonId { get; set; }
    public Lesson? Lesson { get; set; }
}