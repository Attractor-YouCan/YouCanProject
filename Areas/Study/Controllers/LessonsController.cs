using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class LessonsController : Controller
{
    private YouCanContext _db;

    private UserManager<User> _userManager;

    public LessonsController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int subTopicId)
    {
        List<Lesson>? lessons = _db.Lessons.Where(l => l.SubjectId == subTopicId).ToList();
        if (lessons == null)
            return NotFound("NO lessons in this topic !");
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound("No user");

        UserLessons? userLessons = await _db.UserLessons
            .FirstOrDefaultAsync(ul => ul.UserId == currentUser.Id
                                       && ul.SubjectId == subTopicId);
        if (userLessons == null)
        {
            userLessons = new UserLessons() { UserId = currentUser.Id, SubjectId = subTopicId, PassedLevel = 0, IsPassed = true,SubtopicId = null};
            _db.UserLessons.Add(userLessons);
            await _db.SaveChangesAsync();
        }
        ViewBag.CurrentLessonLevel = userLessons.PassedLevel;
        return View(lessons);
    }


    public async Task<IActionResult> Details(int lessonId)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Lesson? lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
        if (lesson == null)
            return NotFound("No Lesson!");
        UserLessons? userLessons = await _db.UserLessons
            .FirstOrDefaultAsync(ul => ul.UserId == currentUser.Id
            && ul.SubtopicId == lesson.SubtopicId);
        if (lesson.LessonLevel > userLessons.PassedLevel + 1)
            return RedirectToAction("Index", new { subTopicId = lesson.SubtopicId });
        if (userLessons == null)
            return NotFound("NO UserLesson!");
        if (userLessons.PassedLevel >= lesson.LessonLevel || userLessons.PassedLevel + 1 == lesson.LessonLevel)
            return View(lesson);
        return NotFound("Пройдите предыдущий урок, чтобы открыть");
    }
}
