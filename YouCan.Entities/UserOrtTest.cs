namespace YouCan.Entities;

public class UserOrtTest : EntityBase
{
    public bool IsPassed { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int? OrtTestId { get; set; }
    public OrtTest? OrtTest { get; set; }

    public int? PassedLevel { get; set; }
    public DateTime? PassedDateTime { get; set; }
    public int? Points { get; set; }
    public int? PassedTimeInMin { get; set; }


}
