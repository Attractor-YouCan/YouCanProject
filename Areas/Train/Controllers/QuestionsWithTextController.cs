using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Train.Dto;
using YouCan.Entities;
using YouCan.Service.Service;
using YouCan.Services;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsWithTextController : Controller
{
    private readonly ICRUDService<Subject> _subjectCrudService;
    private readonly ICRUDService<Test> _testCrudService;
    private readonly W3RootFileManager _w3RootFileManager;
    private readonly UserManager<User> _userManager;
    public QuestionsWithTextController(ICRUDService<Test> testCrudService, ICRUDService<Subject> subjectCrudService, W3RootFileManager w3RootFileManager, UserManager<User> userManager)
    {
        _w3RootFileManager = w3RootFileManager;
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
                if (subject.UserTestType == Entites.Models.UserTestType.Question)
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
                ViewBag.Subject = subject;
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
                if (subject.UserTestType == Entites.Models.UserTestType.Question)
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
                    ViewBag.Subject = subject;
                    return View(test);
                }
            }
        }
        return RedirectToAction("Index", "Test");
    }
}
