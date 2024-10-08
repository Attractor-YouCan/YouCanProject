using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Mvc.Areas.Ort.ViewModels;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Ort.Controllers;
[Area("Ort")]
[Authorize]
public class TestsController : Controller
{
    private ICrudService<OrtTest> _ortTestService;
    private ICrudService<UserOrtTest> _userOrtTestService;
    private UserManager<User> _userManager;
    private readonly IImpactModeService _impactModeService;

    public TestsController(ICrudService<OrtTest> ortTestService,
        ICrudService<UserOrtTest> userOrtTestService,
        UserManager<User> userManager,
        IImpactModeService impactModeService)
    {
        _ortTestService = ortTestService;
        _userOrtTestService = userOrtTestService;
        _userManager = userManager;
        _impactModeService = impactModeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int ortTestId)
    {
        User? curUser = await _userManager.GetUserAsync(User);
        if (curUser == null)
            return RedirectToAction("Login", "Account");
        OrtTest? ortTest = await _ortTestService.GetById(ortTestId);
        if (ortTest == null)
            return NotFound("no ort test");
        ViewBag.OrtTestId = ortTest.Id;
        return View(ortTest.Tests);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] TestSubmissionModel testSubmissionModel)
    {
        OrtTest? ortTest = await _ortTestService.GetById(testSubmissionModel.OrtTestId);
        if (ortTest == null)
            return BadRequest("No OrtTest Id!");

        User? currentUser = await _userManager.GetUserAsync(User);
        UserOrtTest? userOrtTest =  _userOrtTestService
            .GetAll()
            .FirstOrDefault(u => u.UserId == currentUser.Id);
        List<TestAnswersModel> selectedAnswer = testSubmissionModel.SelectedAnswers;
        List<OrtTestModel>? timeSpent = testSubmissionModel.TimeSpent;
        List<Test>? tests = ortTest.Tests;
        List<OrtTestResultModel> ortTestResultModels = new List<OrtTestResultModel>();
        int? testPointSum = 0;
        int passedTimeInMin = 0;
        int? passingTimeInMin = ortTest.Tests.Select(t => t.TimeForTestInMin).Sum();
        foreach (var test in tests)
        {
            int testPassingCount = (
                from question in test.Questions
                from answer in question.Answers.Where(a => a.IsCorrect)
                join selectedAnswerLINQ in selectedAnswer on answer.Id
                    equals selectedAnswerLINQ.AnswerId
                select selectedAnswer
            ).Count();

            int? testPoints = (
                from question in test.Questions
                from answer in question.Answers.Where(a => a.IsCorrect)
                join selectedAnswerLINQ in selectedAnswer on answer.Id
                    equals selectedAnswerLINQ.AnswerId
                select question.Point
            ).Sum();

            double testPassedTime = timeSpent.FirstOrDefault(t => t.TestId == test.Id)!.TimeSpent / 60.0;

            testPointSum += testPoints;
            passedTimeInMin += (int)testPassedTime;
            ortTestResultModels.Add(new OrtTestResultModel()
            {
                RightsCount = testPassingCount,
                QuestionCount = test.Questions.Count,
                TestId = test.Id,
                SpentTimeInMin = testPassedTime,
                PointsSum = testPointSum,
                TestPoints = testPoints
            });
        }

        userOrtTest.OrtTestId = userOrtTest.OrtTestId == null ? ortTest.Id : userOrtTest.OrtTestId;
        userOrtTest.PassedLevel = ortTest.OrtLevel;
        userOrtTest.Points = testPointSum;
        userOrtTest.PassedTimeInMin = passedTimeInMin;
        userOrtTest.PassedDateTime = DateTime.UtcNow;
        int? totalPoints = ortTest.Tests.SelectMany(t => t.Questions).Sum(q => q.Point);
        if (testPointSum >= totalPoints / 2 && passedTimeInMin < passingTimeInMin)
            userOrtTest.IsPassed = true;
        else
            userOrtTest.IsPassed = false;
        await _userOrtTestService.Update(userOrtTest);

        // Return the result data in the response
        return Ok(new { ortTestResultModels });
    }

    [HttpGet]
    public async Task<IActionResult> Result(List<OrtTestResultModel> ortTestResultModels)
    {
        User user = await _userManager.GetUserAsync(User);
        await _impactModeService.UpdateImpactMode(user.StatisticId);
        return View(ortTestResultModels);
    }



    

}