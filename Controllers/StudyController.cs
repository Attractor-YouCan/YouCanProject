using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers;

public class StudyController : Controller
{
    private YouCanDb _db;

    private UserManager<User> _userManager;
    
    // GET
    public StudyController(YouCanDb db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        List<Subtopic> subtopics = _db.Subtopics.ToList();
        return View(subtopics);
    }

    public async Task<IActionResult> Lessons(int subTopicId)
    {
        List<Lesson> lessons = _db.Lessons.Where(l => l.SubtopicId == subTopicId).ToList();
        UserLessons userLessons = new UserLessons(){Id = 1, SubtopicId = subTopicId, LessonId = 3, IsPassed = true, PassedLevel = 4};
        ViewBag.CurrentLessonLevel = userLessons.PassedLevel;
        return View(lessons);
    }

    public async Task<IActionResult> Lesson(int lessonId)
    {
        var lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
        return View(lesson);
    }
}