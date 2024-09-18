using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Mvc.Areas.Ort.ViewModels;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class MiniTestsController : Controller
{
    private ICrudService<Lesson> _lessonService;
    private ICrudService<UserLessons> _userLessonService;
    private ICrudService<Test> _testService;
    private ICrudService<UserLevel> _userLevel;
    private UserManager<User> _userManager;
    private readonly IImpactModeService _impactModeService;

    public MiniTestsController(ICrudService<Lesson> lessonService,
        ICrudService<UserLessons> userLessonService,
        ICrudService<Test> testService, ICrudService<UserLevel> userLevel,
        UserManager<User> userManager, IImpactModeService impactModeService)
    {
        _testService = testService;
        _lessonService = lessonService;
        _userLessonService = userLessonService;
        _userManager = userManager;
        _userLevel = userLevel;
        _impactModeService = impactModeService;
    }

    public async Task<IActionResult> Index(int lessonId)
    {
        Test? test = _testService.GetAll()
            .FirstOrDefault(t => t.LessonId == lessonId);
        if (test == null)
            return NotFound("No Test!");
        return View(test);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] List<TestAnswersModel> selectedAnswers)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Test? test = await _testService.GetById(selectedAnswers[0].TestId);
        Lesson? lesson = await _lessonService.GetById((int)test.LessonId!);
        UserLevel? userLevels = _userLevel.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id && ul.SubjectId == lesson.SubjectId);

        UserLessons? userLesson = _userLessonService.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id && 
                ul.SubjectId == lesson.SubjectId && 
                ul.LessonId == lesson.Id);

        int passingCount = (
            from question in test.Questions
            from answer in question.Answers.Where(a => a.IsCorrect)
            join selectedAnswer in selectedAnswers on answer.Id equals selectedAnswer.AnswerId
            select selectedAnswer
        ).Count();
        bool result = (double)passingCount / test.Questions.Count >= 0.8;
        if (result)
            if (userLevels.Level <= lesson.LessonLevel)
            {
                userLevels.Level = lesson.LessonLevel;
                userLesson.IsPassed = true;
                currentUser.UserExperiences.Add(new UserExperience { UserId = currentUser.Id, Date = DateTime.UtcNow, ExperiencePoints = 5 });
                await _userManager.UpdateAsync(currentUser);
                await _impactModeService.UpdateImpactMode(currentUser.StatisticId);
                await _userLevel.Update(userLevels);
                await _userLessonService.Update(userLesson);
            }
        
        var testResult = new
        {
            isPassed = result,
            lessonId = lesson.Id,
            subtopicId = lesson.SubjectId
        };
        return Ok(testResult);
    }

    [HttpGet]
    public IActionResult Result(bool isPassed, int lessonId, int subtopicId)
    {
        ViewData["LessonId"] = lessonId;
        ViewData["SubtopicId"] = subtopicId;
        return View(isPassed);
    }
}
