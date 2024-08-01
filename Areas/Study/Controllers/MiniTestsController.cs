using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Areas.Study.ViewModels;
using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class MiniTestsController : Controller
{
    private YouCanContext _db;

    private UserManager<User> _userManager;

    public MiniTestsController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int lessonId)
    {
        Test? test = await _db.Tests.Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.LessonId == lessonId);
        if (test == null)
            return NotFound("No Test!");
        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] List<TestAnswersModel> selectedAnswers)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Test? test = await _db.Tests.Include(q => q.Questions)
            .ThenInclude(a => a.Answers)
            .FirstOrDefaultAsync(t => t.Id == selectedAnswers[0].TestId);
        Lesson? lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == test.LessonId);
        UserLessons? userLessons = await _db.UserLessons.FirstOrDefaultAsync(ul =>
            ul.UserId == currentUser.Id && ul.SubjectId == lesson.SubjectId);

        int passingCount = (
            from question in test.Questions
            from answer in question.Answers.Where(a => a.IsCorrect)
            join selectedAnswer in selectedAnswers on answer.Id equals selectedAnswer.AnswerId
            select selectedAnswer
        ).Count();
        int n = 0;
        if (passingCount >= 2)
        {
            userLessons.PassedLevel = lesson.LessonLevel;
            userLessons.IsPassed = true;
            userLessons.LessonId = lesson.Id;
            _db.UserLessons.Update(userLessons);
            n = await _db.SaveChangesAsync();
        }
        var testResult = new
        {
            isPassed = n > 0,
            lessonId = lesson.Id,
            subtopicId = lesson.SubjectId
        };
        return Ok(testResult);
    }

    [HttpGet]
    public IActionResult Result(bool isPassed, int lessonId, int subtopicId)
    {
        ViewBag.LessonId = lessonId;
        ViewBag.SubtopicId = subtopicId;
        return View(isPassed);
    }
}
