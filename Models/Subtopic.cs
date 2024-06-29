namespace YouCan.Models;

public class Subtopic
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int TopicId { get; set; }
    public Topic? Topic { get; set; }

    public List<Lesson> Lessons { get; set; }
    public Subtopic()
    {
        Lessons = new List<Lesson>();
    }
}
