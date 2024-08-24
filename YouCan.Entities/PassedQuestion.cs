namespace YouCan.Entities;

public class PassedQuestion : EntityBase
{
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}