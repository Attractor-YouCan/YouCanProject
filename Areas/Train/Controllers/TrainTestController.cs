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
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        Test? test = await _db.Tests.Include(q => q.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == testId);

        if (test == null)
            return NotFound("No Test!");

        var subjectId = test.SubjectId;

        var answeredQuestionIds = await _db.PassedQuestions
            .Where(pq => pq.UserId == user.Id)
            .Select(pq => pq.QuestionId)
            .ToListAsync();

        var questions = test.Questions
            .Where(q => !answeredQuestionIds.Contains(q.Id))
            .OrderBy(q => q.Id)
            .ToList();

        var count = questions.Count;
        var items = questions.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        if (!items.Any())
            return NotFound("No unanswered questions!");

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
    public async Task<IActionResult> GetNextQuestion([FromForm] int currentPage, [FromForm] int subtopicId)
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
    
        int pageToLoad = currentPage + 1;
    
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
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

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

            _db.PassedQuestions.Add(new PassedQuestion
            {
                QuestionId = question.Id,
                UserId = user.Id
            });
        }

        await _db.SaveChangesAsync();

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
    
    [HttpPost]
    public async Task<IActionResult> ReportQuestion([FromBody] QuestionReportModel model)
    {
        if (ModelState.IsValid)
        {
            var question = await _db.Questions.FindAsync(model.QuestionId);
            if (question == null)
            {
                return NotFound("Question not found!");
            }
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return Unauthorized();
            }

            var report = new QuestionReport
            {
                Text = model.Text,
                QuestionId = model.QuestionId,
                UserId = user.Id
            };

            _db.QuestionReports.Add(report);
            await _db.SaveChangesAsync();

            return Ok();
        }

        return BadRequest("Invalid data!");
    }

}