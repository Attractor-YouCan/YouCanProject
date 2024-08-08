using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Admin.ViewModels;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class StudyController : Controller
{
    private ICRUDService<Subject> _subjectService;
    private ICRUDService<Lesson> _lessonService;
    private ICRUDService<LessonModule> _lessonModuleService;
    private ICRUDService<Test> _testService;
    private ICRUDService<Question> _questionService;
    private ICRUDService<Answer> _answerService;

    public StudyController(ICRUDService<Subject> subjectService, 
        ICRUDService<Lesson> lessonService, 
        ICRUDService<LessonModule> lessonModuleService, 
        ICRUDService<Test> testService, 
        ICRUDService<Question> questionService, 
        ICRUDService<Answer> answerService)
    {
        _subjectService = subjectService;
        _lessonService = lessonService;
        _lessonModuleService = lessonModuleService;
        _testService = testService;
        _questionService = questionService;
        _answerService = answerService;
    }

    public async Task<IActionResult> Index(int? subSubjectId)
    {
        List<Subject> subjects = new List<Subject>();
        if (subSubjectId == null)
        {
            subjects = _subjectService.GetAll().Where(s => s.ParentId == null).ToList();
        }
        else
        {
            subjects = _subjectService.GetAll().Where(s => s.ParentId == subSubjectId).ToList();
        }
        return View(subjects);
    }

    public async Task<IActionResult> AllLessons(int subjectId)
    {
        List<Lesson>? lessons = _lessonService.GetAll()
            .Where(l => l.SubjectId == subjectId).ToList();
        ViewBag.SubjectId = subjectId;
        return View(lessons);
    }

    [HttpGet]
    public async Task<IActionResult> CreateLesson(int subjectId)
    {
        ViewBag.SubjectId = subjectId;
        Subject subject = await _subjectService.GetById(subjectId);
        int? avaibleLevel = subject.Lessons.Select(l => l.LessonLevel).Max();
        ViewBag.AvailableLevel = avaibleLevel + 1;
        return View();
    }
    
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateLesson([FromForm] LessonModel model)
    {
        // Ваша логика обработки данных
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int lessonId)
    {
        Lesson? lesson = await _lessonService.GetById(lessonId);
        LessonModel lessonModel = new LessonModel
        {
            Id = lesson.Id,
            LessonLevel = (int)lesson.LessonLevel,
            ExistsVideoUrl = lesson.VideoUrl,
            LessonTitle = lesson.Title,
            LessonTitle2 = lesson.SubTitle,
            Description = lesson.Description,
            Lecture = lesson.Lecture,
            Modules = lesson.LessonModules.Select(lm => new LessonModuleModel
            {
                ModuleTitle = lm.Title,
                ModuleContent = lm.Content,
                ExistsPhotoUrl = lm.PhotoUrl
            }).ToList()
        };
        return View(lessonModel);
    }

    [HttpPost]    
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] LessonModel model)
    {
        Lesson lesson = await _lessonService.GetById((int)model.Id);
        lesson.Title = model.LessonTitle;
        lesson.SubTitle = model.LessonTitle2;
        lesson.Description = model.Description;
        lesson.Lecture = model.Lecture;
        if (model.LessonVideo is not null)
        {
            
        }

        await _lessonService.Update(lesson);
        return View();
    }
}