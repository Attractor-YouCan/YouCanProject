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
            Statistic = new Statistic { Id = 1, Streak = 5, TotalExperience = 1000, StudyMinutes = new TimeSpan(2, 30, 0) },
            StatisticId = 1
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
