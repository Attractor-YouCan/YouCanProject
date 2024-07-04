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
        User? currentUser = await _userManager.GetUserAsync(User);
        UserLessons? userLessons = null;
        if (currentUser!= null)
            userLessons = await _db.UserLessons.FirstOrDefaultAsync(ul => ul.UserId == currentUser.Id);
        if (userLessons == null)
            ViewBag.CurrentLessonLevel = 0;
        else
            ViewBag.CurrentLessonLevel = userLessons.PassedLevel;
        return View(lessons);
    }

    
    public async Task<IActionResult> Lesson(int lessonId)
    {
        //User? currentUser = await _userManager.GetUserAsync(User);
        var lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
        return View(lesson);
    }

    public async Task<IActionResult> Test(int lessonId)
    {
        Test? test = await _db.Tests.Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.LessonId == lessonId);
        return View(test);
    }
}