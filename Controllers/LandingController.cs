using Microsoft.AspNetCore.Mvc;

namespace YouCan.Controllers;

public class LandingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}