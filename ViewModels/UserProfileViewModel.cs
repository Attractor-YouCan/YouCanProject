using YouCan.Entities;

namespace YouCan.ViewModels;

public class UserProfileViewModel
{
    public User User { get; set; }
    public List<UserLevel> UserLevels { get; set; }
    public List<UserLessonViewModel> UserLessons { get; set; }
}