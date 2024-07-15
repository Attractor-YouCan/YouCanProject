using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Models;

namespace YouCan.Areas.Train.Controllers;
[Area("Train")]
[Authorize]
public class SubTopicController : Controller
{
    private YouCanContext _db;
    private UserManager<User> _userManager;

    public SubTopicController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    public IActionResult Index()
    {
        List<Subtopic> subtopics = _db.Subtopics.Where(sb => sb.Id != 3).ToList();
        return View(subtopics);
    }

}