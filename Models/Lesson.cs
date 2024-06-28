namespace YouCan.Models;

public class Lesson
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string StudyMaterial { get; set; }
    public int RequiredLevel { get; set; }

    public int SubtopicId { get; set; }
    public Subtopic? Subtopic { get; set; }

    public List<Test> Tests { get; set; }
    public Lesson()
    {
        Tests = new List<Test>();
    }
}
