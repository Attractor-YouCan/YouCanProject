namespace YouCan.Entities;

public class Announcement : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
