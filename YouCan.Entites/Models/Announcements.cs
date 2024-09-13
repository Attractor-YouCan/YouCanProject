using YouCan.Entities;

namespace YouCan.Entites.Models;

public class Announcements : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}
