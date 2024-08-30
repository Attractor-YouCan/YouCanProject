using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;

[Area("Admin")]
public class StatController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ICRUDService<Test> _testsManager;
    private readonly ICRUDService<Tariff> _tariffManager;
    public StatController(UserManager<User> userManager, ICRUDService<Test> testManager, ICRUDService<Tariff> tariffManager)
    {
        _userManager = userManager;
        _testsManager = testManager;
        _tariffManager = tariffManager;
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
            .GroupBy(u => u.Tariff.Name ?? "Без тарифа")
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToList();
        ViewBag.Tests = tests;
        ViewBag.Users = users;
        
        ViewBag.TestCount  = tests.Count;
        ViewData["UsersCount"] = _userManager.Users.Count();

        ViewBag.Start = _tariffManager.GetAll()
            .FirstOrDefault(t => t.Name == "Start");
        ViewBag.Pro = _tariffManager.GetAll()
            .FirstOrDefault(t => t.Name == "Pro");
        ViewBag.Premium = _tariffManager.GetAll()
            .FirstOrDefault(t => t.Name == "Premium");

        ViewData["AllNewUsers"] = CalculateAllNewUsers();
        ViewData["NewStartUsers"] = CalcualteStartUsers();
        ViewData["NewProUsers"] = CalculateProUsers();
        ViewData["NewPremiumUsers"] = CalculatePremiumUsers();

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
    public double CalculateProUsers()
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
    public double CalculatePremiumUsers()
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
