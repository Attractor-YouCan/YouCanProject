using Microsoft.AspNetCore.Mvc;

namespace YouCan.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}