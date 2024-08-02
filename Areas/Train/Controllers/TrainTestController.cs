using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YouCan.Areas.Train.ViewModels;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TrainTestController : Controller
{
    private ICRUDService<Subject> _subjectService;
    private ICRUDService<Question> _questionService;
    private ICRUDService<Test> _testService;
    private ICRUDService<QuestionReport> _questionReportService;
    private ICRUDService<PassedQuestion> _passedQuestionsService;
    private UserManager<User> _userManager;


    public TrainTestController(ICRUDService<Subject> subjectService, 
        ICRUDService<QuestionReport> questionReportService,
        ICRUDService<Test> testService, 
        ICRUDService<Question> questionService,
        ICRUDService<PassedQuestion> passedQuestionsService, 
        UserManager<User> userManager)
    {
        _subjectService = subjectService;
        _testService = testService;
        _questionService = questionService;
        _passedQuestionsService = passedQuestionsService;
        _questionReportService = questionReportService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int testId)
    {
        int page = 1;
        int pageSize = 1;
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        Test? test = await _testService.GetById(testId);

        if (test == null)
            return NotFound("No Test!");

        var subjectId = test.SubjectId;

        var answeredQuestionIds = _passedQuestionsService.GetAll()
            .Where(pq => pq.UserId == user.Id)
            .Select(pq => pq.QuestionId)
            .ToList();

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
            SubjectName = _subjectService.GetAll().Where(s => s.Id == subjectId).Select(s => s.Name).FirstOrDefault()
        };

        return View(viewModel);
    }

    
    [HttpPost]
    public async Task<IActionResult> GetNextQuestion([FromForm] int currentPage, [FromForm] int subtopicId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();
        int pageSize = 1; 
        var test = await _testService.GetById(subtopicId);

        if (test == null)
            return NotFound("Test not found!");
        
        var subjectId = test.SubjectId;
        ViewBag.SubjectName = _subjectService.GetAll().Where(s => s.Id == subjectId).Select(s => s.Name).FirstOrDefault();
        var answeredQuestionIds =  _passedQuestionsService.GetAll()
            .Where(pq => pq.UserId == user.Id)
            .Select(pq => pq.QuestionId)
            .ToList();
        var questions = test.Questions
            .Where(q => !answeredQuestionIds.Contains(q.Id))
            .OrderBy(q => q.Id)
            .ToList();
    
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
        var question = await _questionService.GetById(model.QuestionId);
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
            var question = await _questionService.GetById(answer.QuestionId);
            if (question == null)
                continue;

            var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == answer.SelectedAnswerId);
            var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);

            if (selectedAnswer   != null && selectedAnswer.IsCorrect)
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

            await _passedQuestionsService.Insert(new PassedQuestion
            {
                QuestionId = question.Id,
                UserId = user.Id
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
    
    [HttpPost]
    public async Task<IActionResult> ReportQuestion([FromBody] QuestionReportModel model)
    {
        if (ModelState.IsValid)
        {
            var question = await _questionService.GetById(model.QuestionId);
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

            await _questionReportService.Insert(report);
            return Ok();
        }

        return BadRequest("Invalid data!");
    }

}