namespace YouCan.Models;

public class LessonModule
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? PhotoUrl { get; set; }
    public int? LessonId { get; set; }
    public Lesson? Lesson { get; set; }
}