using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Areas.Train.Controllers;
[Area("Train")]
[Authorize]
public class TestController : Controller
{
    private YouCanContext _context;

    public TestController(YouCanContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int subtopicId)
    {
        var tests = await _context.Tests.Where(t => t.SubtopicId == subtopicId && t.LessonId == null).ToListAsync();
        return View(tests);
    }
}