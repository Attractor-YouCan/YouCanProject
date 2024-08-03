using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TestController : Controller
{
    private ICRUDService<Test> _testService;

    public TestController(ICRUDService<Test> testService)
    {
        _testService = testService;
    }

    public async Task<IActionResult> Index(int subSubjectId)
    {
        var tests = _testService.GetAll()
            .Where(t => t.SubjectId == subSubjectId && t.LessonId == null).ToList();
        return View(tests);
    }
}