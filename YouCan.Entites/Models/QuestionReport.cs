
namespace YouCan.Entities;

public class QuestionReport
{
    public int Id { get; set; }
    public string Text { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
    
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}