namespace YouCan.Models;

public class UserORTTests
{
    public int Id { get; set; }
    public bool IsPassed { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }

    public int TestId { get; set; }
    public Test? Test { get; set; }


}
