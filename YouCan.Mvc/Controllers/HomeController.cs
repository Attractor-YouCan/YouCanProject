using Microsoft.AspNetCore.Mvc;

namespace YouCan.Mvc.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> TermsOfUse()
    {
        return View();
    }

    public async Task<IActionResult> PrivacyPolicy()
    {
        return View();
    }
}