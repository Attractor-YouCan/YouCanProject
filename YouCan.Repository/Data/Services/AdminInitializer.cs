using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YouCan.Entities;

namespace YouCan.Services;

public class AdminInitializer
{
    public static async Task SeedAdminUser(RoleManager<IdentityRole<int>> _roleManager, UserManager<User> _userManager)
    {
        string adminEmail = "admin@admin.com";
        string adminUsername = "admin";
        string adminPassword = "Admin1!";
        string path = "/userImages/defProf-ProfileN=1.png";
        string fullName = "Admin";

        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            var superadmin = new User()
            {
                Email = adminEmail,
                UserName = adminUsername,
                AvatarUrl = path,
                FullName = fullName,
                BirthDate = DateTime.UtcNow,
                Disctrict = "",
                PhoneNumber = "0"
            };
            IdentityResult result = await _userManager.CreateAsync(superadmin, adminPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(superadmin, "admin");
            }
        }
        User user1 = new User()
        {
            Email = "qwe@qwe",
            UserName = "qwe",
            AvatarUrl = "/userImages/defProf-ProfileN=1.png",
            FullName = "Anton",
            BirthDate = DateTime.Today.ToUniversalTime(),
            Disctrict = "Bishkek",
            PhoneNumber = "1"
        };
        User user2 = new User()
        {
            Email = "asd@asd",
            UserName = "asd",
            AvatarUrl = "/userImages/defProf-ProfileN=1.png",
            FullName = "SynAntona",
            BirthDate = DateTime.Today.ToUniversalTime(),
            Disctrict = "Osh",
            PhoneNumber = "2"
        };
        User user3 = new User()
        {
            Email = "zxc@zxc",
            UserName = "zxc",
            AvatarUrl = "/userImages/defProf-ProfileN=1.png",
            FullName = "John Ceena",
            BirthDate = DateTime.Today.ToUniversalTime(),
            Disctrict = "Issyk-Kul",
            PhoneNumber = "3"
        };
        User profileTest = new User()
        {
            Email = "ewq@ewq",
            UserName = "Alex",
            AvatarUrl = "/userImages/defProf-ProfileN=1.png",
            FullName = "Qwe Qwe",
            BirthDate = DateTime.Today.ToUniversalTime(),
            Disctrict = "qweqweqeq",
            PhoneNumber = "4",
            Statistic = new Statistic { Id = 1, Streak = 5, StudyMinutes = new TimeSpan(2, 30, 0) },
            StatisticId = 1,
            UserExperiences = {
            new UserExperience {Id = 1, UserId = 5, Date = new DateTime(2024, 8, 24, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 100 },
            new UserExperience {Id = 2, UserId = 5, Date = new DateTime(2024, 8, 23, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 75 },
            new UserExperience {Id = 3, UserId = 5, Date = new DateTime(2024, 8, 25, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 10 },
            new UserExperience {Id = 4, UserId = 5, Date = new DateTime(2024, 8, 26, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 50 },
            new UserExperience {Id = 5, UserId = 5, Date = new DateTime(2024, 8, 27, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 100 },
            new UserExperience {Id = 6, UserId = 5, Date = new DateTime(2024, 8, 18, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 35 },
            new UserExperience {Id = 7, UserId = 5, Date = new DateTime(2024, 8, 29, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 75 }
            }
        };
        List<User> users = new List<User>() { user1, user2, user3, profileTest };
        foreach (var user in users)
        {
            IdentityResult resultOfUser = await _userManager.CreateAsync(user, "qweAa1_");
            if (resultOfUser.Succeeded)
                await _userManager.AddToRoleAsync(user, "user");
        }
    }
}
