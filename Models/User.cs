using Microsoft.AspNetCore.Identity;

namespace YouCan.Models;

public class User : IdentityUser<int>
{
    public string? PhotoUrl { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? Age { get; set; }
    public string? District { get; set; }
    
}