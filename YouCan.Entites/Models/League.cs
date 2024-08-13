namespace YouCan.Entities;

public class League : EntityBase
{
    public string LeagueName { get; set; }
    public int MinPoints { get; set; }
    public int MaxPoints { get; set; }
    public ICollection<User> Users { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}