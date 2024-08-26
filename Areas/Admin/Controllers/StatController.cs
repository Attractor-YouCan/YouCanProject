using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;

[Area("Admin")]
public class StatController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ICRUDService<Test> _testsManager;
    public StatController(UserManager<User> userManager, ICRUDService<Test> testManager)
    {
        _userManager = userManager;
        _testsManager = testManager; 
    }
    [Route("[area]/[controller]/")]
    public async Task<IActionResult> Stat()
    {
        var tests = _testsManager.GetAll()
            .GroupBy(t => t.Subject?.Name ?? "Без урока")
            .Select(g => new {Name = g.Key, Count = g.Count()})
            .ToList();
        var users = _userManager.Users
            .Include(u => u.Tariff)
            .GroupBy(u => u.Tariff.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToList();

        ViewBag.Tests = tests;
        ViewBag.Users = users;
        
        ViewBag.TestCount  = tests.Count;
        ViewBag.UserCount = _userManager.Users.Count(); 

        ViewBag.Tariff1 = new {Name = users[0].Name, Count = users[0].Count };
        ViewBag.Tariff2 = new {Name = users[1].Name, Count = users[1].Count};
        ViewBag.Tariff3 = new {Name = users[2].Name, Count = users[2].Count};

        ViewBag.AllNewUsers = CalculateAllNewUsers();
        ViewBag.NewStartUsers = CalcualteStartUsers();
        ViewBag.NewProUsers = CalcualteProUsers();
        ViewBag.NewPremiumUsers = CalcualtePremiumUsers();

        return View();
    }
    public double CalculateAllNewUsers()
    {
        var users = _userManager.Users.ToList();
        var newUsersThisMonth = _userManager.Users.Where(u => DateTime.UtcNow - u.CreatedAt < TimeSpan.FromDays(30)).ToList();

        double usersCount = users.Count;
        double newUsersCount = newUsersThisMonth.Count;

        double percent = newUsersCount / usersCount * 100;
        return percent;
    }
    public double CalcualteStartUsers()
    {
        double usersCount = _userManager.Users.Count();
        var startTariffUsers = _userManager.Users
            .Include(u => u.Tariff)
            .Where(u => u.Tariff.Name == "Start" && u.TariffStartDate - DateTime.UtcNow < TimeSpan.FromDays(30))
            .ToList();

        double startCount = startTariffUsers.Count;

        double percent = startCount / usersCount * 100;
        return percent;
    }
    public double CalcualteProUsers()
    {
        double usersCount = _userManager.Users.Count();
        var proTariffUsers = _userManager.Users
            .Include(u => u.Tariff)
            .Where(u => u.Tariff.Name == "Pro" && u.TariffStartDate - DateTime.UtcNow < TimeSpan.FromDays(30))
            .ToList();

        double proCount = proTariffUsers.Count;

        double percent = proCount / usersCount * 100;
        return percent;
    }
    public double CalcualtePremiumUsers()
    {
        double usersCount = _userManager.Users.Count();
        var premiumTariffUsers = _userManager.Users
            .Include(u => u.Tariff)
            .Where(u => u.Tariff.Name == "Premium" && u.TariffStartDate - DateTime.UtcNow < TimeSpan.FromDays(90))
            .ToList();

        double premiumCount = premiumTariffUsers.Count;

        double percent = premiumCount / usersCount * 100;
        return percent;
    }
}
