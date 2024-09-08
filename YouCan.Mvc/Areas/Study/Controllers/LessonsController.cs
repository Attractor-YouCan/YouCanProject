using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Mvc.Areas.Study.Dto;
using YouCan.Service;
using YouCan.Entities;
using Microsoft.Extensions.Localization;

namespace YouCan.Mvc.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class LessonsController : Controller
{
    private ICrudService<Lesson> _lessonService;
    private ICrudService<UserLessons> _userLessonService;
    private UserManager<User> _userManager;
    private readonly ICrudService<UserLevel> _userLevel;
    private IStringLocalizer<LessonsController> _localizer;
    private ICrudService<LessonTime> _lessonTimeService;
    public LessonsController(ICrudService<Lesson> lessonService,
        ICrudService<UserLessons> userLessonService,
        ICrudService<UserLevel> userLevel,
        ICrudService<LessonTime> lessonTimeService,
        UserManager<User> userManager,
        IStringLocalizer<LessonsController> localizer)
    {
        _lessonService = lessonService;
        _userLessonService = userLessonService;
        _lessonTimeService = lessonTimeService;
        _userManager = userManager;
        _localizer = localizer;
        _userLevel = userLevel;
    }

    public async Task<IActionResult> Index(int subTopicId)
    {
        List<Lesson>? lessons = _lessonService.GetAll().Where(l => l.SubjectId == subTopicId).ToList();
        if (lessons == null)
            return NotFound("NO lessons in this topic !");
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound("No user");

        UserLevel? userLevel = _userLevel.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id && ul.SubjectId == subTopicId);
        if (userLevel == null)
        {
            userLevel = new()
            {
                UserId = currentUser.Id,
                SubjectId = subTopicId,
                Level = 0,
            };
            await _userLevel.Insert(userLevel);
        }
        ViewBag.CurrentLessonLevel = userLevel.Level;
        return View(lessons);
    }


    public async Task<IActionResult> Details(int lessonId)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Lesson? lesson = await _lessonService.GetById(lessonId);
        if (lesson == null)
            return NotFound("No Lesson!");
        UserLevel? userLevel = _userLevel.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id
            && ul.SubjectId == lesson.SubjectId);
        if (lesson.LessonLevel > userLevel.Level + 1)
            return RedirectToAction("Index", new { subTopicId = lesson.SubjectId });
        if (userLevel == null)
            return NotFound("NO UserLesson!");
        if (userLevel.Level >= lesson.LessonLevel || userLevel.Level + 1 == lesson.LessonLevel)
        {
            if (_userLessonService.GetAll().FirstOrDefault(ul => ul.UserId == currentUser.Id && ul.LessonId == lesson.Id) == null)
            {
                UserLessons userLesson = new()
                {
                    UserId = currentUser.Id,
                    LessonId = lesson.Id,
                    IsPassed = false,
                    SubjectId = lesson.SubjectId
                };
                await _userLessonService.Insert(userLesson);
            }
            return View(lesson);
        }
        return NotFound(_localizer["NotUnlocked"]);
    }
    [HttpPost]
    public async Task<IActionResult> LogTime([FromBody] LessonTimeDto model)
    {
        if(ModelState.IsValid)
        {
            var lesson = _lessonService.GetById(model.LessonId);
            if(lesson is null)
            {
                return NotFound($"Lesson with id: {model.LessonId} not found");
            }
            var user = await _userManager.GetUserAsync(User);
            var lessonTime = new LessonTime()
            {
                LessonId = model.LessonId,
                UserId = user.Id,
                TimeSpent = TimeSpan.FromMilliseconds(model.TimeSpent),
                Date = DateOnly.FromDateTime(DateTime.Now),
            };
            await _lessonTimeService.Insert(lessonTime);
            return Ok();
        }
        return BadRequest();
    }
}
