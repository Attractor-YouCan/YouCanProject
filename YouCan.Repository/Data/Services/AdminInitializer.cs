﻿using Microsoft.AspNetCore.Identity;
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
    PhoneNumber = "1",
    Rank = 1,
    UserLessonScore = 1200,  // Silver League
    LeagueId = 2
};

User user2 = new User()
{
    Email = "asd@asd", 
    UserName = "asd", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "SynAntona",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Osh",
    PhoneNumber = "2",
    Rank = 2,
    UserLessonScore = 300,  // Bronze League
    LeagueId = 1
};

User user3 = new User()
{
    Email = "zxc@zxc", 
    UserName = "zxc", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "John Ceena",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Issyk-Kul",
    PhoneNumber = "3",
    Rank = 3,
    UserLessonScore = 2500,  // Gold League
    LeagueId = 3
};

User user4 = new User()
{
    Email = "mno@mno", 
    UserName = "mno", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "Dwayne Johnson",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Naryn",
    PhoneNumber = "4",
    Rank = 4,
    UserLessonScore = 1800,  // Silver League
    LeagueId = 2
};

User user5 = new User()
{
    Email = "pqr@pqr", 
    UserName = "pqr", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "Emma Watson",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Talas",
    PhoneNumber = "5",
    Rank = 5,
    UserLessonScore = 3500,  // Platinum League
    LeagueId = 4
};

User user6 = new User()
{
    Email = "stu@stu", 
    UserName = "stu", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "Robert Downey Jr.",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Jalal-Abad",
    PhoneNumber = "6",
    Rank = 6,
    UserLessonScore = 4100,  // Diamond League
    LeagueId = 5
};

User user7 = new User()
{
    Email = "vwx@vwx", 
    UserName = "vwx", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "Scarlett Johansson",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Batken",
    PhoneNumber = "7",
    Rank = 7,
    UserLessonScore = 2700,  // Gold League
    LeagueId = 3
};

User user8 = new User()
{
    Email = "yz@yz", 
    UserName = "yz", 
    AvatarUrl = "/userImages/defProf-ProfileN=1.png", 
    FullName = "Tom Holland",
    BirthDate = DateTime.Today.ToUniversalTime(),
    Disctrict = "Chui",
    PhoneNumber = "8",
    Rank = 8,
    UserLessonScore = 900,  // Bronze League
    LeagueId = 1
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
            ImpactModeStart = new DateTime(2024, 8, 26, 0, 0, 0, DateTimeKind.Utc),
            ImpactModeEnd = new DateTime(2024, 9, 01, 0, 0, 0, DateTimeKind.Utc),
            UserExperiences = {
            new UserExperience {Id = 1, UserId = 5, Date = new DateTime(2024, 8, 26, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 100 },
            new UserExperience {Id = 2, UserId = 5, Date = new DateTime(2024, 8, 27, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 75 },
            new UserExperience {Id = 3, UserId = 5, Date = new DateTime(2024, 8, 28, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 10 },
            new UserExperience {Id = 4, UserId = 5, Date = new DateTime(2024, 8, 29, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 50 },
            new UserExperience {Id = 5, UserId = 5, Date = new DateTime(2024, 8, 30, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 100 },
            new UserExperience {Id = 6, UserId = 5, Date = new DateTime(2024, 8, 31, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 35 },
            new UserExperience {Id = 7, UserId = 5, Date = new DateTime(2024, 9, 01, 0, 0, 0, DateTimeKind.Utc), ExperiencePoints = 75 }
            }
        };

        List<User> users = new List<User>() { user1, user2, user3, user4, user5, user6, user7, user8, profileTest };
        foreach (var user in users)
        {
            IdentityResult resultOfUser = await _userManager.CreateAsync(user, "Qwe123");
            if (resultOfUser.Succeeded)
                await _userManager.AddToRoleAsync(user, "user");
        }
    }
}
