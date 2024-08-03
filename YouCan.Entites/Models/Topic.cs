namespace YouCan.Entities;

public class Topic : EntityBase
{
    public string Name { get; set; }

    public List<Subtopic> Subtopics { get; set; }
    public Topic()
    {
        Subtopics = new List<Subtopic>();
    }
}
