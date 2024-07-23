namespace YouCan.Models;

public class UserOrtTest
{
    public int Id { get; set; }
    public bool IsPassed { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public int? OrtTestId { get; set; }
    public OrtTest? OrtTest { get; set; }
    
    public int? PassedLevel { get; set; }
    public DateTime? PassedDateTime { get; set; }
    public int? Points { get; set; }
    public TimeSpan? PassedTime { get; set; }


}
