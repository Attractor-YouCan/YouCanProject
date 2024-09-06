using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.Admin.ViewModels;
using YouCan.Entities;
using YouCan.Service.Service;
using YouCan.Services;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
public class ORTController : Controller
{
    private readonly ICRUDService<OrtTest> _ortManager;
    private readonly ICRUDService<Test> _testsManager;
    private readonly ICRUDService<OrtInstruction> _instructionsManager;
    private readonly UserManager<User> _userManager;
    private readonly ICRUDService<Subject> _subjectManager;
    private readonly IWebHostEnvironment _env;
    private readonly ICRUDService<Question> _questionManager;
    private readonly ICRUDService<Answer> _answerManager;
    public ORTController(UserManager<User> userManager, ICRUDService<Test> testManager,
                         ICRUDService<OrtInstruction> instructionsManager, ICRUDService<OrtTest> ortManager,
                         ICRUDService<Subject> subjectManager, IWebHostEnvironment env,
                         ICRUDService<Question> questionManager, ICRUDService<Answer> answerManager)
    {
        _userManager = userManager;
        _testsManager = testManager;
        _instructionsManager = instructionsManager;
        _ortManager = ortManager;
        _subjectManager = subjectManager;
        _env = env;
        _questionManager = questionManager;
        _answerManager = answerManager;
    }
    public IActionResult Index() => View(_ortManager.GetAll().ToList());
    public async Task<IActionResult> Details(int ortId)
    {
        var ort = await  _ortManager.GetById(ortId);
        if (ort != null)
        {
            return View(ort);   
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int ortId)
    {
        var ort = await _ortManager.GetById(ortId);
        if (ort != null)
        {
            return View(ort);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Edit(OrtTest ort)
    {
        if (ModelState.IsValid)
        {
            await _ortManager.Update(ort);
            return RedirectToAction("Details", new {ortId = ort.Id});   
        }
        return BadRequest();
    }

    [HttpGet]
    public IActionResult CreateTest(int ortId)
    {
        ViewBag.ortId = ortId;
        ViewBag.OrtInstructions = _instructionsManager.GetAll().ToList();
        ViewData["Subjects"] = _subjectManager.GetAll().ToList();   
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateTest(Test test, string? description)
    {
        if (test != null)
        {
            await _testsManager.Insert(test);
            OrtInstruction instruction = new()
            {
                TestId = test.Id,
                Description = description,
                TimeInMin = test.TimeForTestInMin
            };
            await _instructionsManager.Insert(instruction);
            test.OrtInstructionId = instruction.Id;
            await _testsManager.Update(test);
            return RedirectToAction("Details", new { ortId = test.OrtTestId });
        }
        return BadRequest();
    }

    [HttpGet]
    public IActionResult CreateQuestions(int testId)
    {
        ViewBag.TestId = testId;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateQuestions([FromForm] TestsModel model)
    {
        W3RootFileManager fileManager = new(_env);
        if (model != null)
        {
            var test = await _testsManager.GetById(model.TestId);
            if (test != null)
            {
                if (model.Questions != null)
                {
                    foreach (var question in model.Questions)
                    {
                        string? questionFileName = null;
                        if (question.Image != null)
                        {
                            questionFileName = await fileManager.SaveFormFileAsync("ortTestsResourses", question.Image);
                        }
                        Question newQuestion = new()
                        {
                            Instruction = question.Instruction,
                            Content = question.Text,
                            ImageUrl = questionFileName,
                            IsPublished = true,
                            Point = 1,
                            TestId = model.TestId,
                            SubjectId = test.SubjectId
                        };
                        await _questionManager.Insert(newQuestion);

                        foreach (var asnwer in question.Answers)
                        {
                            Answer newAnswer = new()
                            {
                                Content = asnwer.AnswerText,
                                IsCorrect = asnwer.IsCorrect,
                                QuestionId = newQuestion.Id,
                            };
                            await _answerManager.Insert(newAnswer);
                        }
                    }
                    return RedirectToAction("Details", new {ortId = test.OrtTestId});
                }
            }
        }
        return NotFound();
    }
}
