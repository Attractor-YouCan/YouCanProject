using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YouCan.Areas.Train.ViewModels;
using YouCan.Models;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TrainTestController : Controller
{
    private YouCanContext _db;
    private UserManager<User> _userManager;

    public TrainTestController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int testId)
    {
        int page = 1;
        int pageSize = 1; 
        Test? test = await _db.Tests.Include( q => q.Questions)
            .ThenInclude(q => q.Answers)          
            .FirstOrDefaultAsync(t => t.Id == testId);
        var subjectId = test.SubjectId;
        if (test == null)
            return NotFound("No Test!");
        
        var questions = test.Questions.OrderBy(q => q.Id).ToList();
        var count = questions.Count;
        var items = questions.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        if (!items.Any())
            return NotFound("Question not found!");

        var viewModel = new TestViewModel
        {
            PageViewModel = new PageViewModel(count, page, pageSize),
            CurrentQuestion = items.FirstOrDefault(),
            TestId = testId,
            SubjectName = _db.Subjects.Where(s => s.Id == subjectId).Select(s => s.Name).FirstOrDefault()
        };

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetNextQuestion([FromForm] int currentPage, [FromForm] int subtopicId, [FromForm] int wantedPage)
    {
        int pageSize = 1; 
        var test = await _db.Tests.Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == subtopicId);

        if (test == null)
            return NotFound("Test not found!");
        
        var subjectId = test.SubjectId;
        ViewBag.SubjectName = _db.Subjects.Where(s => s.Id == subjectId).Select(s => s.Name).FirstOrDefault();
        var questions = test.Questions.OrderBy(q => q.Id).ToList();
    
        int pageToLoad = wantedPage > 0 ? wantedPage : currentPage + 1;
    
        if (pageToLoad > questions.Count)
        {
            return Json(new { finished = true });
        }
    
        var question = questions.Skip((pageToLoad - 1) * pageSize).Take(pageSize).FirstOrDefault();

        if (question == null)
            return BadRequest("No more questions!");

        return PartialView("_QuestionPartial", question);
    }
    
    [HttpPost]
    public async Task<IActionResult> CheckAnswer([FromBody] AnswerCheckModel model)
    {
        var question = await _db.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == model.QuestionId);
        if (question == null)
            return NotFound("Question not found!");

        var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == model.SelectedAnswerId);
        if (selectedAnswer == null)
            return NotFound("Answer not found!");

        bool isCorrect = selectedAnswer.IsCorrect;
        return Json(new { isCorrect });
    }

    [HttpPost]
    public async Task<IActionResult> FinishTest([FromBody] FinishTestModel model)
    {
        int correctAnswers = 0;
        var results = new List<QuestionResultViewModel>();

        foreach (var answer in model.Answers)
        {
            var question = await _db.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == answer.QuestionId);
            if (question == null)
                continue;

            var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == answer.SelectedAnswerId);
            var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

            if (selectedAnswer != null && selectedAnswer.IsCorrect)
            {
                correctAnswers++;
            }

            results.Add(new QuestionResultViewModel
            {
                QuestionId = question.Id,
                QuestionContent = question.Content,
                SelectedAnswerId = selectedAnswer?.Id ?? 0,
                CorrectAnswerId = correctAnswer?.Id ?? 0,
                IsCorrect = selectedAnswer?.IsCorrect ?? false,
                Answers = question.Answers.Select(a => new AnswerViewModel
                {
                    Id = a.Id,
                    Text = a.Text
                }).ToList()
            });
        }

        var resultModel = new TestResultViewModel
        {
            CorrectAnswers = correctAnswers,
            Questions = results
        };

        return Json(new { resultModel = resultModel });
    }
        
    [HttpGet]
    public IActionResult TestResult(string resultModel)
    {
        var model = JsonConvert.DeserializeObject<TestResultViewModel>(resultModel);
        return View(model);
    }
}