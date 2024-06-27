namespace YouCan.Models;

public class Topic
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<Subtopic> Subtopics { get; set; }
    public Topic()
    {
        Subtopics = new List<Subtopic>();
    }
}
