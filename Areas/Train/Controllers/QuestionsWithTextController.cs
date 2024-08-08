using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Train.Dto;
using YouCan.Models;
using YouCan.Services;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsWithTextController : Controller
{
    private readonly YouCanContext _youCanContext;
    private readonly W3RootFileManager _w3RootFileManager;
    private readonly UserManager<User> _userManager;
    public QuestionsWithTextController(YouCanContext youCanContext, W3RootFileManager w3RootFileManager, UserManager<User> userManager)
    {
        _youCanContext = youCanContext;
        _w3RootFileManager = w3RootFileManager;
        _userManager = userManager;
    }
    [HttpGet]
    public IActionResult Create(int subSubjectId)
    {
        if (ModelState.IsValid)
        {
            bool subjectIsParent = _youCanContext.Subjects.Any(e => e.ParentId == subSubjectId);
            if (!subjectIsParent)
            {
                var subject = _youCanContext.Subjects.FirstOrDefault(e => e.Id == subSubjectId);
                if (subject is not null)
                {
                    if (!subject.UserTestIsTest)
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
        }
        return RedirectToAction("Index", "Train");
    }
    [HttpPost]
    public async Task<IActionResult> Create(TestDto test)
    {
        if (ModelState.IsValid)
        {
            bool subjectIsParent = _youCanContext.Subjects.Any(e => e.ParentId == test.SubjectId);
            if (!subjectIsParent)
            {
                var subject = _youCanContext.Subjects.FirstOrDefault(e => e.Id == test.SubjectId);
                if (subject is not null)
                {
                    if (test.Questions.All(e => e.Answers.Count == 4))
                    {
                        var dbTest = new Test()
                        {
                            SubjectId = test.SubjectId,
                            Text = test.Text,
                            CreatorUserId = int.Parse(_userManager.GetUserId(User)!),
                            Questions = new()
                        };
                        foreach(var question in test.Questions)
                        {
                            var dbQuestion = new Question()
                            {
                                Test = dbTest,
                                Text = question.Text,
                                Instruction = string.Empty,
                                Answers = [],
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
                        _youCanContext.Tests.Add(dbTest);
                        await _youCanContext.SaveChangesAsync();
                        return RedirectToAction("Index", "Train");
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
        }
        return RedirectToAction("Index", "Train");
    }
}
