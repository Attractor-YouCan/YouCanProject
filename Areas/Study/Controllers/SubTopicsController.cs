using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Areas.Study.ViewModels;
using YouCan.Models;

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

    public IActionResult Index(int? subSubjectId)
    {
        List<Subject> subjects = new List<Subject>();
        if (subSubjectId == null)
        {
            subjects = _db.Subjects.Where(s => s.ParentId == null).ToList();
        }
        else
        {
            subjects = _db.Subjects.Where(s => s.ParentId == subSubjectId).ToList();
        }
        return View(subjects);
    }

}