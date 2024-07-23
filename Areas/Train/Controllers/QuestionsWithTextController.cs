using Microsoft.AspNetCore.Mvc;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsWithTextController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
