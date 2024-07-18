using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IActionResult> Index(int testId, bool? isAnalogy, int page = 1)
    {
        int pageSize = 1; 
        Test? test = await _db.Tests.Include( q => q.Questions)
            .ThenInclude(q => q.Answers)          
            .FirstOrDefaultAsync(t => t.Id == testId);

        if (test == null)
            return NotFound("No Test!");

        if ( isAnalogy == true)
        {
            // Логика для аналогии (если есть)
        }

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
        };

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> GetNextQuestion([FromForm] int currentPage, [FromForm] int subtopicId)
    {
        int pageSize = 1; 
        var test = await _db.Tests.Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == subtopicId);

        if (test == null)
            return NotFound("Test not found!");

        var questions = test.Questions.OrderBy(q => q.Id).ToList();
        var nextPage = currentPage + 1;
        var nextQuestion = questions.Skip((nextPage - 1) * pageSize).Take(pageSize).FirstOrDefault();

        if (nextQuestion == null)
            return BadRequest("No more questions!");

        return PartialView("_QuestionPartial", nextQuestion);
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

        foreach (var answer in model.Answers)
        {
            var question = await _db.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == answer.QuestionId);
            if (question == null)
                continue;

            var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == answer.SelectedAnswerId);
            if (selectedAnswer != null && selectedAnswer.IsCorrect)
            {
                correctAnswers++;
            }
        }

        return Json(new { correctAnswers });
    }
}