namespace YouCan.Models;

public class PassedQuestion
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}