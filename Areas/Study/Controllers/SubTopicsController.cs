using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class SubTopicsController : Controller
{
    private YouCanContext _db;

    private UserManager<User> _userManager;

    public SubTopicsController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        List<Subtopic> subtopics = _db.Subtopics.ToList();
        return View(subtopics);
    }

}