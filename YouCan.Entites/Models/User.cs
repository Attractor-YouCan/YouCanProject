using Microsoft.AspNetCore.Identity;
using YouCan.Entites.Models;
using YouCan.Entities;

namespace YouCan.Entities;

public class User : IdentityUser<int>
{
    public string AvatarUrl { get; set; }
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public string Disctrict { get; set; }
    public DateTime? ImpactModeStart { get; set; }
    public DateTime? ImpactModeEnd { get; set; }
    public int? TariffId { get; set; }
    public Tariff? Tariff { get; set; }
    public DateTime TariffStartDate { get; set; } = DateTime.UtcNow;
    public DateTime? TariffEndDate { get; set; }
    
    public int? LeagueId { get; set; }
    public League? League { get; set; }
    
    public int Rank { get; set; } 

    public int UserLessonScore { get; set; }
    public int? StatisticId { get; set; }
    public Statistic? Statistic { get; set; }
    public List<UserExperience>? UserExperiences { get; set; }
    public List<UserLessons>? Lessons { get; set; }
    public List<UserLevel>? UserLevels { get; set; }
    public List<UserOrtTest>? Tests { get; set; }
    public List<Question>? Questions { get; set; }

    public User()
    {
        Questions = new List<Question>();
        Lessons = new List<UserLessons>();
        UserLevels = new List<UserLevel>();
        UserExperiences = new List<UserExperience>();
        Tests = new();
    }
}
