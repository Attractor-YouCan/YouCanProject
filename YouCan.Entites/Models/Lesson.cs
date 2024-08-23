namespace YouCan.Entities;

public class Lesson : EntityBase
{
    public string? Title { get; set; }
    public string? SubTitle { get; set; }
    public string? VideoUrl { get; set; }
    public string? Description { get; set; }
    public string? Lecture { get; set; }
    public List<LessonModule>? LessonModules { get; set; }
    
    public int RequiredLevel { get; set; }
    public int? LessonLevel { get; set; }
    //Привязка внешнего ключа к Subject
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }

    public List<Test> Tests { get; set; }
    public Lesson()
    {
        Tests = new List<Test>();
        LessonModules = new List<LessonModule>();
    }
}
