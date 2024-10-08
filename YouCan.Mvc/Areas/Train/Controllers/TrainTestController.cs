using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Mvc.Areas.Train.ViewModels;
using YouCan.Service;
using YouCan.Entities;

namespace YouCan.Mvc.Areas.Train.Controllers;

[Area("Train")]
[Authorize]
public class TrainTestController : Controller
{
    private ICrudService<PassedQuestion> _passedQuestionService;
    private ICrudService<Subject> _subjectService;
    private ICrudService<Test> _testService;
    private ICrudService<Question> _questionService;
    private ICrudService<QuestionReport> _questionReportService;
    private UserManager<User> _userManager;
    private readonly IImpactModeService _impactModeService;


    public TrainTestController(ICrudService<PassedQuestion> passedQuestionService,
        ICrudService<Subject> subjectService,
        ICrudService<QuestionReport> questionReportService,
        ICrudService<Test> testService,
        ICrudService<Question> questionService,
        UserManager<User> userManager, IImpactModeService impactModeService)
    {
        _passedQuestionService = passedQuestionService;
        _questionReportService = questionReportService;
        _subjectService = subjectService;
        _testService = testService;
        _questionService = questionService;
        _userManager = userManager;
        _impactModeService = impactModeService;
    }

    public async Task<IActionResult> Index(int subSubjectId)
    {
        int page = 1;
        int pageSize = 1;
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        var subjectId = subSubjectId;


        var answeredQuestionIds = _passedQuestionService.GetAll()
            .Where(pq => pq.UserId == user.Id)
            .Select(pq => pq.QuestionId)     
            .ToList();

        var questions = _questionService.GetAll()
            .Where(q => !answeredQuestionIds.Contains(q.Id) && q.SubjectId == subSubjectId && q.IsPublished)
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
            SubjectId = subSubjectId,
            SubjectName = _subjectService.GetAll()
                .Where(s => s.Id == subjectId)
                .Select(s => s.Name)
                .FirstOrDefault()
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
        var test = _testService.GetAll().FirstOrDefault(t => t.SubjectId == subtopicId);

        if (test == null)
            return NotFound("Test not found!");

        var subjectId = test.SubjectId;
        ViewBag.SubjectName = _subjectService.GetAll()
            .Where(s => s.Id == subjectId)
            .Select(s => s.Name)
            .FirstOrDefault()!;

        var answeredQuestionIds = _passedQuestionService.GetAll()
            .Where(pq => pq.UserId == user.Id)
            .Select(pq => pq.QuestionId)
            .ToList();
        var questions = test.Questions
            .Where(q => !answeredQuestionIds.Contains(q.Id))
            .OrderBy(q => q.Id)
            .ToList();

        int pageToLoad = currentPage + 1;

        if (pageToLoad > questions.Count && pageToLoad < 0)
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
        var user = await _userManager.GetUserAsync(User);

        var question = await _questionService.GetById(model.QuestionId);
        if (question == null)
            return NotFound("Question not found!");

        var selectedAnswer = question.Answers.FirstOrDefault(a => a.Id == model.SelectedAnswerId);
        if (selectedAnswer == null)
            return NotFound("Answer not found!");

        bool isCorrect = selectedAnswer.IsCorrect;
        if (isCorrect)
        {
            user.UserExperiences.Add(new UserExperience { UserId = user.Id, Date = DateTime.UtcNow, ExperiencePoints = 1 });


            await _userManager.UpdateAsync(user);

            await _impactModeService.UpdateImpactMode(user.StatisticId);

        }
        return Json(new { isCorrect });
    }

    [HttpPost]
    public async Task<IActionResult> FinishTest([FromBody] FinishTestModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        var answeredQuestionIds = _passedQuestionService.GetAll()
            .Where(pq => pq.UserId == user.Id)
            .Select(pq => pq.QuestionId)
            .ToList();

        foreach (var answer in model.Answers)
        {
            if (answeredQuestionIds.Contains(answer.QuestionId))
            {
                continue;
            }

            var question = await _questionService.GetById(answer.QuestionId);
            if (question == null)
                continue;

            await _passedQuestionService.Insert(new PassedQuestion
            {
                QuestionId = question.Id,
                UserId = user.Id
            });
        }
        return Ok();
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