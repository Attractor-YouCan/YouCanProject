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
    private ICrudService<LessonTime> _lessonTimeService;
    private IStringLocalizer<AccountController> _localizer;
    public LessonsController(ICrudService<Lesson> lessonService,
        ICrudService<UserLessons> userLessonService,
        ICrudService<LessonTime> lessonTimeService,
        UserManager<User> userManager,
        IStringLocalizer<LessonsController> localizer)
    {
        _lessonService = lessonService;
        _userLessonService = userLessonService;
        _lessonTimeService = lessonTimeService;
        _userManager = userManager;
        _localizer = localizer;
    }

    public async Task<IActionResult> Index(int subTopicId)
    {
        List<Lesson>? lessons = _lessonService.GetAll().Where(l => l.SubjectId == subTopicId).ToList();
        if (lessons == null)
            return NotFound("NO lessons in this topic !");
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return NotFound("No user");

        UserLessons? userLessons =  _userLessonService.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id
                                       && ul.SubjectId == subTopicId);
        if (userLessons == null)
        {
            userLessons = new UserLessons() { UserId = currentUser.Id, SubjectId = subTopicId, PassedLevel = 0, IsPassed = true };
            await _userLessonService.Insert(userLessons);
        }
        ViewBag.CurrentLessonLevel = userLessons.PassedLevel;
        return View(lessons);
    }


    public async Task<IActionResult> Details(int lessonId)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        Lesson? lesson = await _lessonService.GetById(lessonId);
        if (lesson == null)
            return NotFound("No Lesson!");
        UserLessons? userLessons = _userLessonService.GetAll()
            .FirstOrDefault(ul => ul.UserId == currentUser.Id
            && ul.SubjectId == lesson.SubjectId);
        if (lesson.LessonLevel > userLessons.PassedLevel + 1)
            return RedirectToAction("Index", new { subTopicId = lesson.SubjectId });
        if (userLessons == null)
            return NotFound("NO UserLesson!");
        if (userLessons.PassedLevel >= lesson.LessonLevel || userLessons.PassedLevel + 1 == lesson.LessonLevel)
            return View(lesson);
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
