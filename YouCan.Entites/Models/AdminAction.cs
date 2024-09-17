using YouCan.Entities;

namespace YouCan.Entites.Models;

public class AdminAction : EntityBase
{
    public int Id { get; set; }
    public string Action { get; set; }
    public DateTime ExecuteTime { get; set; } = DateTime.UtcNow;
    public string Details { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
