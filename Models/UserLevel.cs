namespace YouCan.Models;

public class UserLevel
{
    public int Id { get; set; }
    public int Level { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int SubtopicId { get; set; }
    public Subtopic? Subtopic { get; set; }
}
