using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
                PhoneNumber = "0",
                TariffId = 3,
                TariffStartDate = DateTime.UtcNow,
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
            TariffId = 2,
            TariffStartDate = DateTime.UtcNow
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
            TariffId = 1,
            TariffStartDate = DateTime.UtcNow
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
            TariffId = 1,
            TariffStartDate = DateTime.UtcNow
        };
        List<User> users = new List<User>() { user1, user2, user3 };
        foreach (var user in users)
        {
            IdentityResult resultOfUser = await _userManager.CreateAsync(user, "Qwe123");
            if (resultOfUser.Succeeded)
                await _userManager.AddToRoleAsync(user, "user");
        }
    }
}
