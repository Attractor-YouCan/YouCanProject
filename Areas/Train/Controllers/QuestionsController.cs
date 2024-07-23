using Microsoft.AspNetCore.Mvc;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
