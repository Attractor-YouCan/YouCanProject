using Microsoft.AspNetCore.Mvc;

namespace YouCan.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}