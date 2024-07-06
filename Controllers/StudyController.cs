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
        List<Lesson>? lessons = _db.Lessons.Where(l => l.SubtopicId == subTopicId).ToList();
        if (lessons == null)
            return NotFound("NO lessons in this topic !");
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound("No user");
        
        UserLessons? userLessons = await _db.UserLessons
            .FirstOrDefaultAsync(ul => ul.UserId == currentUser.Id
                                       && ul.SubtopicId == subTopicId);
        if (userLessons == null)
        {
            userLessons = new UserLessons() { UserId = currentUser.Id, SubtopicId = subTopicId, PassedLevel = 0, IsPassed = true};
            _db.UserLessons.Add(userLessons);
            await _db.SaveChangesAsync();
        }
        ViewBag.CurrentLessonLevel = userLessons.PassedLevel;
        return View(lessons);
    }

    
    public async Task<IActionResult> Lesson(int lessonId)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound("No User!");
        Lesson? lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
        if (lesson == null)
            return NotFound("No Lesson!");
        UserLessons? userLessons = await _db.UserLessons
            .FirstOrDefaultAsync(ul => ul.UserId == currentUser.Id
            && ul.SubtopicId == lesson.SubtopicId);
        if (lesson.LessonLevel > userLessons.PassedLevel+1)
            return RedirectToAction("Lessons", new {subTopicId = lesson.SubtopicId});
        if (userLessons == null)
            return NotFound("NO UserLesson!");
        if (userLessons.PassedLevel >= lesson.LessonLevel || userLessons.PassedLevel+1 == lesson.LessonLevel)
            return View(lesson);
        return NotFound("Пройдите предыдущий урок, чтобы открыть");
    }

    public async Task<IActionResult> Test(int lessonId)
    {
        Test? test = await _db.Tests.Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.LessonId == lessonId);
        if (test == null)
            return NotFound("No Test!");
        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Test([FromBody] List<TestAnswersModel> selectedAnswers)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Test? test = await _db.Tests.Include(q => q.Questions)
            .ThenInclude(a => a.Answers)
            .FirstOrDefaultAsync(t => t.Id == selectedAnswers[0].TestId);
        Lesson? lesson = await _db.Lessons.FirstOrDefaultAsync(l => l.Id == test.LessonId);
        UserLessons? userLessons = await _db.UserLessons.FirstOrDefaultAsync(ul =>
            ul.UserId == currentUser.Id && ul.SubtopicId == lesson.SubtopicId);

        int passingCount = (
            from question in test.Questions
            from answer in question.Answers.Where(a => a.IsCorrect)
            join selectedAnswer in selectedAnswers on answer.Id equals selectedAnswer.AnswerId
            select selectedAnswer
        ).Count();
        int n =0;
        if (passingCount >=2)
        {
            userLessons.PassedLevel = lesson.LessonLevel;
            userLessons.IsPassed = true;
            userLessons.LessonId = lesson.Id;
            _db.UserLessons.Update(userLessons);
            n = await _db.SaveChangesAsync();
        }
        var testResult = new {
            isPassed = n > 0,
            lessonId = lesson.Id,
            subtopicId = lesson.SubtopicId
        };
        return Ok(testResult);    
    }

    [HttpGet]
    public async Task<IActionResult> TestResultPage(bool isPassed, int lessonId, int subtopicId)
    {
        ViewBag.LessonId = lessonId;
        ViewBag.SubtopicId = subtopicId;
        return View(isPassed);
    }

}