using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TrainController : Controller
{
    private UserManager<User> _userManager;

    public TrainController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
}