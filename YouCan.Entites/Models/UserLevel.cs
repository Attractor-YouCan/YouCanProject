namespace YouCan.Entities;

public class UserLevel : EntityBase
{
    public int Level { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int SubtopicId { get; set; }
    public Subtopic? Subtopic { get; set; }
}
