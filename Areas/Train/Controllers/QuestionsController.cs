using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Train.Dto;
using YouCan.Models;
using YouCan.Services;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
public class QuestionsController : Controller
{
    private readonly YouCanContext _youCanContext;
    private readonly W3RootFileManager _w3RootFileManager;
    public QuestionsController(YouCanContext youCanContext, W3RootFileManager w3RootFileManager)
    {
        _youCanContext = youCanContext;
        _w3RootFileManager = w3RootFileManager;
    }
    [HttpGet]
    public IActionResult Create(int subSubjectId)
    {
        if(ModelState.IsValid)
        {
            bool subjectIsParent = _youCanContext.Subjects.Any(e => e.ParentId == subSubjectId);
            if (subjectIsParent)
            {
                var subject = _youCanContext.Subjects.FirstOrDefault(e => e.Id == subSubjectId);
                if(subject is not null)
                {
                    var model = new QuestionDto()
                    {
                        Answers = new List<AnswerDto>(),
                        SubjectId = subSubjectId
                    };
                    for (int i = 0; i < 5; i++)
                    {
                        model.Answers.Add(new AnswerDto());
                    }
                    ViewBag.Subject = subject;
                    return View(model);
                }
            }
        }
        return RedirectToAction("Index", "Train");
    }
    [HttpPost]
    public async Task<IActionResult> Create(QuestionDto question)
    {
        if (ModelState.IsValid)
        {
            bool subjectIsParent = _youCanContext.Subjects.Any(e => e.ParentId == question.SubjectId);
            if (subjectIsParent)
            {
                var subject = _youCanContext.Subjects.FirstOrDefault(e => e.Id == question.SubjectId);
                if (subject is not null)
                {
                    if(question.Answers.Count == 4)
                    {
                        var dbQuestion = new Question()
                        {
                            SubjectId = question.SubjectId,
                            Text = question.Text,
                            Answers = [],
                            AnswersIsImage = question.AnswerIsImage
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
                        _youCanContext.Questions.Add(dbQuestion);
                        await _youCanContext.SaveChangesAsync();
                        return RedirectToAction("Train", "Index");
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
        }
        return RedirectToAction("Index", "Train");
    }
}
