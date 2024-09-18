using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TestController : Controller
{
    private ICrudService<Test> _testService;

    public TestController(ICrudService<Test> testService)
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