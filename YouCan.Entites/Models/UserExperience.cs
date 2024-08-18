using YouCan.Entities;


public class UserExperience
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime Date { get; set; }
    public int ExperiencePoints { get; set; }
}
