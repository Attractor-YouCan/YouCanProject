namespace YouCan.Models;

public class Test
{
    public int Id { get; set; }
    public int GainingExperience { get; set; }
    public TimeSpan TimeForTest { get; set; }

    public int? LessonId { get; set; }
    public Lesson? Lesson { get; set; }

    public List<Question> Questions { get; set; }
    public Test()
    {
        Questions = new List<Question>();
    }
}
