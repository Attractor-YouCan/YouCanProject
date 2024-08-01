using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TrainController : Controller
{
    private YouCanContext _db;
    private UserManager<User> _userManager;

    public TrainController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
}