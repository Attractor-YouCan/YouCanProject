using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Areas.Study.ViewModels;
using YouCan.Models;
using YouCan.ViewModels;
using YouCan.ViewModels.Account;

namespace YouCan.Areas.ORT.Controllers;
[Area("ORT")]
[Authorize]
public class TestsController : Controller
{
    private YouCanContext _db;
    private UserManager<User> _userManager;

    public TestsController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int ortTestId)
    {
        User? curUser = await _userManager.GetUserAsync(User);
        if (curUser == null)
            RedirectToAction("Login", "Account");
        OrtTest? ortTest = await _db.OrtTests
            .Include(t => t.Tests)
                .ThenInclude(t =>t .Subject)
            .Include(t => t.Tests)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == ortTestId);
        if (ortTest == null)
            return NotFound("no ort test");
        ViewBag.OrtTestId = ortTest.Id;
        UserOrtTest? userOrtTest = await _db.UserORTTests
            .Include(t => t.OrtTest)
            .FirstOrDefaultAsync(t => t.UserId == curUser.Id);
        if (userOrtTest.PassedLevel + 1 < ortTest.OrtLevel)
            return RedirectToAction("Index", "OrtTests");
        return View(ortTest.Tests);
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] TestSubmissionModel testSubmissionModel)
    {
        OrtTest? ortTest = await _db.OrtTests
            .Include(t => t.Tests)
                .ThenInclude(t => t.Questions)
                    .ThenInclude(t => t.Answers)
            .FirstOrDefaultAsync(t => t.Id == testSubmissionModel.OrtTestId);
        if (ortTest == null)
            return BadRequest("No OrtTest Id!");

        User? currentUser = await _userManager.GetUserAsync(User);
        UserOrtTest userOrtTest = await _db.UserORTTests
            .FirstOrDefaultAsync(t => t.UserId == currentUser.Id);
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

        userOrtTest!.OrtTestId = ortTest.Id;
        userOrtTest.PassedLevel = ortTest.OrtLevel;
        userOrtTest.Points = testPointSum;
        userOrtTest.PassedTimeInMin = passedTimeInMin;
        userOrtTest.PassedDateTime = DateTime.UtcNow;
        int? totalPoints = ortTest.Tests.SelectMany(t => t.Questions).Sum(q => q.Point);
        if (testPointSum >= totalPoints/2 && passedTimeInMin < passingTimeInMin)
            userOrtTest.IsPassed = true;
        else
            userOrtTest.IsPassed = false;
        _db.UserORTTests.Update(userOrtTest);
        await _db.SaveChangesAsync();
        // Return the result data in the response
        return Ok(new { ortTestResultModels });
    }

    [HttpGet]
    public IActionResult Result(List<OrtTestResultModel> ortTestResultModels)
    {
        return View(ortTestResultModels);
    }



    

}