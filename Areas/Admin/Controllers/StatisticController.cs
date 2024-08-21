using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;

namespace YouCan.Areas.Admin.Controllers;

public class StatisticController : Controller
{
    private readonly UserManager<User> _userManager;

    public StatisticController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }


}
