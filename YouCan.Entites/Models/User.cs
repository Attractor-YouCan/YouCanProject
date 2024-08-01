using Microsoft.AspNetCore.Identity;

namespace YouCan.Entities;

public class User : IdentityUser<int>
{
    public string AvatarUrl { get; set; }
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public string Disctrict { get; set; }
    public Statistic? Statistic { get; set; }
    public List<UserLessons>? Lessons { get; set; }
    public List<UserLevel>? UserLevels { get; set; }
    public List<UserOrtTest>? Tests { get; set; }
    public List<Question>? Questions { get; set; }

    public User()
    {
        Questions = new List<Question>();
        Lessons = new List<UserLessons>();
        UserLevels = new List<UserLevel>();
        Tests = new();
    }
}
