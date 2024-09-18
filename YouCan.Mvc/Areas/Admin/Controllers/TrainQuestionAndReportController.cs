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
    private ICrudService<Subject> _subjectService;
    private ICrudService<Question> _questionService;
    private ICrudService<QuestionReport> _questionReportService;
    private ICrudService<Answer> _answerService;
    private UserManager<User> _userManager;

    public TrainQuestionAndReportController(ICrudService<Subject> subjectService,
        UserManager<User> userManager,
        ICrudService<Question> questionService,
        ICrudService<Answer> answerService,
        ICrudService<QuestionReport> questionReportService)
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
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var question = await _questionService.GetById(id);
        if (question == null)
        {
            return NotFound();
        }

        var subjects = _subjectService.GetAll().Where(s => s.SubjectType == SubjectType.Child).ToList();
        var selectedSubject = subjects.FirstOrDefault(s => s.Id == question.SubjectId);
        if (selectedSubject != null)
        {
            subjects.Remove(selectedSubject);
            subjects.Insert(0, selectedSubject);  
        }

        ViewBag.Subjects = subjects; 
        return View(question);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Question model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Subjects = _subjectService.GetAll();
            return View(model);
        }

        // Обновляем данные вопроса
        var question = await _questionService.GetById(Convert.ToInt32(model.Id));
        if (question == null)
        {
            return NotFound();
        }

        question.Content = model.Content;
        question.Instruction = model.Instruction;
        question.SubjectId = model.SubjectId;
        question.ImageUrl = model.ImageUrl;

        // Обновляем ответы
        for (int i = 0; i < model.Answers.Count; i++)
        {
            question.Answers[i].Content = model.Answers[i].Content;
            question.Answers[i].IsCorrect = model.Answers[i].IsCorrect;
        }

        await _questionService.Update(question);
        return RedirectToAction("Details", new { id = question.Id });
    }

}