using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Models;

namespace YouCan.Controllers;

public class StudyController : Controller
{
    private YouCanDb _db;

    private UserManager<User> _userManager;
    
    // GET
    public StudyController(YouCanDb db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        List<Lesson> lessons = _db.Lessons.ToList();
        return View(lessons);
    }
}