using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Mvc.Areas.Train.Dto;
using YouCan.Mvc.Services;
using YouCan.Service;
using YouCan.Entities;
using System.Globalization;

namespace YouCan.Mvc.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsWithTextController : Controller
{
    private readonly ICrudService<Subject> _subjectCrudService;
    private readonly ICrudService<Test> _testCrudService;
    private readonly UserManager<User> _userManager;
    public QuestionsWithTextController(ICrudService<Test> testCrudService, ICrudService<Subject> subjectCrudService, W3RootFileManager w3RootFileManager, UserManager<User> userManager)
    {
        _userManager = userManager;
        _subjectCrudService = subjectCrudService;
        _testCrudService = testCrudService;
    }
    [HttpGet]
    public async Task<IActionResult> CreateAsync(int subSubjectId)
    {
        if (ModelState.IsValid)
        {
            var subject = await _subjectCrudService.GetById(subSubjectId);
            if (subject is not null && subject.SubjectType == SubjectType.Child)
            {
                if (subject.UserTestType == Entities.UserTestType.Question)
                {
                    return RedirectToAction("Create", "QuestionsWithText", new { SubSubjectId = subSubjectId });
                }
                var model = new TestDto()
                {
                    Questions = new List<TestDto.TestQuestionDto>()
                    {
                        new TestDto.TestQuestionDto()
                        {
                            Answers = new()
                            {
                                string.Empty,
                                string.Empty,
                                string.Empty,
                                string.Empty,
                            }
                        }
                    },
                    SubjectId = subSubjectId,
                };
                LoadViewBag(subject);
                return View(model);
            }
        }
        return RedirectToAction("Index", "Test");
    }
    [HttpPost]
    public async Task<IActionResult> Create(TestDto test)
    {
        if (ModelState.IsValid)
        {
            var subject = await _subjectCrudService.GetById(test.SubjectId);
            if (subject is not null && subject.SubjectType == SubjectType.Child)
            {
                if (subject.UserTestType == Entities.UserTestType.Question)
                {
                    return RedirectToAction("Create", "QuestionsWithText", new { SubSubjectId = test.SubjectId });
                }
                if (test.Questions.All(e => e.Answers.Count == 4))
                {
                    var dbTest = new Test()
                    {
                        SubjectId = test.SubjectId,
                        Text = test.Text,
                        UserId = int.Parse(_userManager.GetUserId(User)!),
                        Questions = new()
                    };
                    foreach(var question in test.Questions)
                    {
                        var dbQuestion = new Question()
                        {
                            Test = dbTest,
                            Content = question.Text,
                            Instruction = string.Empty,
                            Answers = new(),
                            AnswersIsImage = false,
                        };
                        foreach(var answer in question.Answers)
                        {
                            var dbAnswer = new Answer()
                            {
                                Content = answer,
                                Question = dbQuestion,
                            };
                            dbQuestion.Answers.Add(dbAnswer);
                        }
                        dbQuestion.Answers[question.CorrectAnswerId].IsCorrect = true;
                        dbTest.Questions.Add(dbQuestion);
                    }
                    await _testCrudService.Insert(dbTest);
                    return RedirectToAction("Index", "Test");
                }
                else
                {
                    foreach(var question in  test.Questions)
                    {
                        for (int i = question.Answers.Count; i < 4; i++)
                        {
                            question.Answers.Add(string.Empty);
                        }
                    }
                    ModelState.AddModelError("", "Ваши задачи должна содержать 4 вариантов ответа");
                    LoadViewBag(subject);
                    return View(test);
                }
            }
        }
        return RedirectToAction("Index", "Test");
    }
    public async Task<IActionResult> Created(int testId)
    {
        if (ModelState.IsValid)
        {
            var test = await _testCrudService.GetById(testId);
            if (test is not null)
            {
                if (!test.IsPublished && test.UserId.HasValue && test.UserId == int.Parse(_userManager.GetUserId(User)!))
                {
                    return View();
                }
            }
        }
        return RedirectToAction("Index", "Test");
    }
    private void LoadViewBag(Subject subject)
    {
        ViewBag.Subject = subject;
        SubjectLocalization localization = (subject.SubjectLocalizations?.Where(e => e.Culture == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).FirstOrDefault()) ?? (subject.SubjectLocalizations?.FirstOrDefault());
        ViewBag.SubjectTitle = localization?.Title ?? subject.Name;
        ViewBag.SubjectSubtitle = localization?.Subtitle;
        ViewBag.SubjectDescription = localization?.Description;
    }
}
