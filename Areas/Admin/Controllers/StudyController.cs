using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Admin.ViewModels;
using YouCan.Entities;
using YouCan.Service.Service;
using YouCan.Services;

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
    private IWebHostEnvironment _environment;

    public StudyController(ICRUDService<Subject> subjectService, 
        ICRUDService<Lesson> lessonService, 
        ICRUDService<LessonModule> lessonModuleService, 
        ICRUDService<Test> testService, 
        ICRUDService<Question> questionService, 
        ICRUDService<Answer> answerService,
        IWebHostEnvironment environment)
    {
        _subjectService = subjectService;
        _lessonService = lessonService;
        _lessonModuleService = lessonModuleService;
        _testService = testService;
        _questionService = questionService;
        _answerService = answerService;
        _environment = environment;
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
        int? availableLevel = subject.Lessons.Select(l => l.LessonLevel).Max();
        ViewBag.AvailableLevel = availableLevel == null ? 1 : availableLevel + 1;
        return View();
    }
    
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateLesson([FromForm] LessonModel? model)
    {
        W3RootFileManager fileManager = new W3RootFileManager(_environment);
        if (model != null)
        {
            Lesson lesson = new Lesson()
            {
                Title = model.LessonTitle,
                SubTitle = model.LessonTitle2,
                Description = model.Description,
                Lecture = model.Lecture,
                LessonLevel = model.LessonLevel,
                RequiredLevel = model.LessonLevel-1,
                SubjectId = model.SubjectId,
                VideoUrl = model.LessonVideo
            };
            await _lessonService.Insert(lesson);
            Test test = new Test()
            {
                SubjectId = model.SubjectId,
                LessonId = lesson.Id
            };
            await _testService.Insert(test);
            await _lessonService.Update(lesson);
            if (model.Modules != null)
                foreach (var module in model.Modules)
                {
                    string? moduleFileName = null;
                    if (module.ModulePhoto != null)
                        moduleFileName = await fileManager.SaveFormFileAsync("lessonResources", module.ModulePhoto);
                    LessonModule newModule = new LessonModule()
                    {
                        Title = module.ModuleTitle,
                        Content = module.ModuleContent,
                        PhotoUrl = moduleFileName,
                        LessonId = lesson.Id
                    };
                    await _lessonModuleService.Insert(newModule);
                    await _lessonService.Update(lesson);
                }

            if (model.Questions != null)
                foreach (var question in model.Questions)
                {
                    string? questionFileName = null;
                    if (question.Image != null)
                        questionFileName = await fileManager.SaveFormFileAsync("lessonResources", question.Image);
                    Question newQuestion = new Question()
                    {
                        Instruction = question.Instruction,
                        Content = question.Text, 
                        ImageUrl = questionFileName,
                        IsPublished = true,
                        Point = 1,
                        SubjectId = model.SubjectId,
                        TestId = test.Id
                    };
                    await _questionService.Insert(newQuestion);
                    await _testService.Update(test);
                    foreach (var answer in question.Answers)
                    {
                        Answer newAnswer = new Answer()
                        {
                            Content = answer.AnswerText,
                            IsCorrect = answer.IsCorrect,
                            QuestionId = newQuestion.Id
                        };
                        await _answerService.Insert(newAnswer);
                        await _questionService.Update(newQuestion);
                    }
                }
            await _lessonService.Update(lesson);
            return Ok(new {subjectId = lesson.SubjectId});
        }
        ViewBag.SubjectId = model.SubjectId;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int lessonId)
    {
        Lesson? lesson = await _lessonService.GetById(lessonId);
        List<QuestionModel> lessonQuestion = new List<QuestionModel>();
        foreach (var test in lesson.Tests)
            foreach (var question in test.Questions)
                lessonQuestion.Add(new QuestionModel()
                {
                    Instruction = question.Instruction,
                    Text = question.Content,
                    Answers = question.Answers.Select(a => new AnswerModel()
                    {
                        AnswerText = a.Content,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                });
        
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
            }).ToList(),
            Questions = lessonQuestion
        };
        return View(lessonModel);
    }

    [HttpPost]    
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Edit([FromForm] LessonModel model)
    {
        W3RootFileManager fileManager = new W3RootFileManager(_environment);
        Lesson? lesson = await _lessonService.GetById((int)model.Id);
        if (model != null)
        {
            lesson.Title = model.LessonTitle;
            lesson.SubTitle = model.LessonTitle2;
            lesson.Description = model.Description;
            lesson.Lecture = model.Lecture;
            lesson.VideoUrl = model.LessonVideo;
            Test? test = await _testService.GetById(lesson.Tests.First().Id);
            lesson.LessonModules.Clear();
            await _lessonService.Update(lesson);
            foreach (var module in lesson.LessonModules)
            {
                fileManager.DeleteFile(module.PhotoUrl);
                await _lessonModuleService.DeleteById(module.Id);
            }
            foreach (var module in model.Modules)
            {
                string? moduleFileName = module.ExistsPhotoUrl;
                if (module.ModulePhoto != null)
                    moduleFileName = await fileManager.SaveFormFileAsync("lessonResources", module.ModulePhoto);
                LessonModule newModule = new LessonModule()
                {
                    Title = module.ModuleTitle,
                    Content = module.ModuleContent,
                    PhotoUrl = moduleFileName,
                    LessonId = lesson.Id
                };
                await _lessonModuleService.Insert(newModule);
                await _lessonService.Update(lesson);
            }
            test.Questions.Clear();
            await _testService.Update(test);
            foreach (var question in test.Questions)
            {
                fileManager.DeleteFile(question.ImageUrl);
                question.Answers.Clear();
                await _questionService.Update(question);
                foreach (var answer in question.Answers)
                    await _answerService.DeleteById(answer.Id);
                await _questionService.DeleteById(question.Id);
            }
            foreach (var question in model.Questions)
            {
                string? questionFileName = question.QuestionExistsPhotoUrlElement;
                if (question.Image != null)
                    questionFileName = await fileManager.SaveFormFileAsync("lessonResources", question.Image);
                Question newQuestion = new Question()
                {
                    Instruction = question.Instruction,
                    Content = question.Text, 
                    ImageUrl = questionFileName,
                    IsPublished = true,
                    Point = 1,
                    SubjectId = lesson.SubjectId,
                    TestId = test.Id
                };
                await _questionService.Insert(newQuestion);
                await _testService.Update(test);
                foreach (var answer in question.Answers)
                {
                    Answer newAnswer = new Answer()
                    {
                        Content = answer.AnswerText,
                        IsCorrect = answer.IsCorrect,
                        QuestionId = newQuestion.Id
                    };
                    await _answerService.Insert(newAnswer);
                    await _questionService.Update(newQuestion);
                }
            }
            await _lessonService.Update(lesson);
            return Ok(new {subjectId = lesson.SubjectId});
        }
        ViewBag.SubjectId = model.SubjectId;
        return View(model);
    }
}