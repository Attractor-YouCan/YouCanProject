using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class TrainQuestionAndReportController : Controller
{
    private ICRUDService<Subject> _subjectService;
    private ICRUDService<Question> _questionService;
    private ICRUDService<QuestionReport> _questionReportService;
    private ICRUDService<Answer> _answerService;
    private UserManager<User> _userManager;

    public TrainQuestionAndReportController(ICRUDService<Subject> subjectService,
        UserManager<User> userManager,
        ICRUDService<Question> questionService,
        ICRUDService<Answer> answerService,
        ICRUDService<QuestionReport> questionReportService)
    {
        _questionReportService = questionReportService;
        _subjectService = subjectService;
        _userManager = userManager;
        _questionService = questionService;
        _answerService = answerService;
    }

    public async Task<IActionResult> Subject(int? subSubjectId)
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

    public async Task<IActionResult> Question(int subSubjectId)
    {
        var questions = _questionService.GetAll().Where(q => q.IsPublished == false && q.SubjectId == subSubjectId).ToList();
        return View(questions);
    }

    public async Task<IActionResult> Details(int id)
    {
        var question = await _questionService.GetById(id);
        return View(question);
    }
    
    // Publish question
    [HttpPost]
    public async Task<IActionResult> Publish(int id)
    {
        var question = await _questionService.GetById(id);
        if (question == null)
        {
            return NotFound();
        }

        question.IsPublished = true;
        await _questionService.Update(question);
        return RedirectToAction(nameof(Subject));
    }

    // Delete question and answers
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var question = await _questionService.GetById(id);
        if (question == null)
        {
            return NotFound();
        }

        //_answerService.DeleteRange(question.Answers);
        await _questionService.DeleteById(id);

        return RedirectToAction(nameof(Subject));
    }

    public async Task<IActionResult> Report()
    {
        var reports = _questionReportService.GetAll().ToList();
        return View(reports);
    }
}