using YouCan.Entities;

namespace YouCan.Mvc.ViewModels.Account;

public class UserProfileViewModel
{
    public User User { get; set; }
    public List<UserLevel> UserLevels { get; set; }
    public List<UserLessonViewModel> UserLessons { get; set; }
}