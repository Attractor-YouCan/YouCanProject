using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Train.Dto;
using YouCan.Entities;
using YouCan.Repository.Repository;
using YouCan.Service.Service;
using YouCan.Services;
using static System.Net.Mime.MediaTypeNames;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class QuestionsController : Controller
{
    private readonly ICRUDService<Subject> _subjectCrudService;
    private readonly ICRUDService<Question> _questionCrudService;
    private readonly W3RootFileManager _w3RootFileManager;
    private readonly UserManager<User> _userManager; 
    public QuestionsController(ICRUDService<Question> questionCrudService, ICRUDService<Subject> subjectCrudService, W3RootFileManager w3RootFileManager, UserManager<User> userManager)
    {
        _w3RootFileManager = w3RootFileManager;
        _userManager = userManager;
        _subjectCrudService = subjectCrudService;
        _questionCrudService = questionCrudService;
    }
    [HttpGet]
    public async Task<IActionResult> CreateAsync(int subSubjectId)
    {
        if(ModelState.IsValid)
        {
            var subject = await _subjectCrudService.GetById(subSubjectId);
            if(subject is not null && subject.SubjectType == SubjectType.Child)
            {
                if(subject.UserTestType == Entites.Models.UserTestType.Test)
                {
                    return RedirectToAction("Create", "QuestionsWithText", new { SubSubjectId = subSubjectId });
                }
                var model = new QuestionDto()
                {
                    Answers = new List<AnswerDto>(),
                    SubjectId = subSubjectId
                };
                for (int i = 0; i < 4; i++)
                {
                    model.Answers.Add(new AnswerDto());
                }
                ViewBag.Subject = subject;
                return View(model);
            }
        }
        return RedirectToAction("Index", "Test");
    }
    [HttpPost]
    public async Task<IActionResult> Create(QuestionDto question)
    {
        if (ModelState.IsValid)
        {
            var subject = await _subjectCrudService.GetById(question.SubjectId);
            if (subject is not null && subject.SubjectType == SubjectType.Child)
            {
                if (subject.UserTestType == Entites.Models.UserTestType.Test)
                {
                    return RedirectToAction("Create", "QuestionsWithText", new { SubSubjectId = question.SubjectId });
                }
                if (question.Answers.Count == 4)
                {
                    var dbQuestion = new Question()
                    {
                        SubjectId = question.SubjectId,
                        Content = question.Text,
                        Instruction = question.Instruction,
                        Answers = [],
                        AnswersIsImage = question.AnswerIsImage,
                        UserId = int.Parse(_userManager.GetUserId(User)),
                    };
                    if(question.Image is not null)
                    {
                        dbQuestion.ImageUrl = await _w3RootFileManager.SaveFormFileAsync("questionImages", question.Image);
                    }
                    if(question.AnswerIsImage)
                    {
                        dbQuestion.Answers = question.Answers.Select(async e => new Answer
                        {
                            Content = await _w3RootFileManager.SaveFormFileAsync("answerImages", e.Image),
                            Question = dbQuestion,
                        }).Select(t => t.Result).ToList();
                    }
                    else
                    {
                        dbQuestion.Answers = question.Answers.Select(e => new Answer
                        {
                            Content = e.Text,
                            Question = dbQuestion,
                        }).ToList();
                    }
                    dbQuestion.Answers[question.CorrectAnswerId].IsCorrect = true;
                    await _questionCrudService.Insert(dbQuestion);
                    return RedirectToAction("Index", "Test");
                }
                else if(question.Answers.Count > 4)
                {
                    ModelState.AddModelError("", "Ваша задача должна содержать 4 вариантов ответа");
                    ViewBag.Subject = subject;
                    return View(question);
                }
                else
                {
                    for(int i = question.Answers.Count; i<4; i++ )
                    {
                        question.Answers.Add(new AnswerDto());
                    }
                    ModelState.AddModelError("", "Ваша задача должна содержать 4 вариантов ответа");
                    ViewBag.Subject = subject;
                    return View(question);
                }
            }
        }
        return RedirectToAction("Index", "Test");
    }
}
