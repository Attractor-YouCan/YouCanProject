namespace YouCan.Entities;

public class Subtopic : EntityBase
{
    public string Name { get; set; }
    public string? ImageUrl { get; set; }

    public int TopicId { get; set; }
    public Topic? Topic { get; set; }

    public List<Lesson> Lessons { get; set; }
    public Subtopic()
    {
        Lessons = new List<Lesson>();
    }
}
