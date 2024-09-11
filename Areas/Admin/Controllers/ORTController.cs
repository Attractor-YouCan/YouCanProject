using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing;
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

    [HttpGet]
    public IActionResult CreateOrt()
    {
        ViewBag.MaxOrtLevel = _ortManager.GetAll().Max(o => o.OrtLevel);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrt(OrtTest ort)
    {
        if (ModelState.IsValid)
        {
            await _ortManager.Insert(ort);
            return RedirectToAction("Index");   
        }
        return BadRequest();
    }

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

            var ort = await _ortManager.GetById(test.OrtTestId.Value);
            if (ort != null)
            {
                ort.TimeForTestInMin += test.TimeForTestInMin;
                await _ortManager.Update(ort);
            }

            return RedirectToAction("Details", new { ortId = test.OrtTestId });
        }
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> CreateQuestions(int testId)
    {
        var test = await _testsManager.GetById(testId);
        ViewBag.OrtId = test.OrtTestId;
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

    [HttpGet]
    public async Task<IActionResult> EditTest(int testId)
    {
        var test = await _testsManager.GetById(testId);

        TestsModel testModel = new()
        {
            TestId = testId,
            Text = test.Text,
            TimeForTestInMin = test.TimeForTestInMin,
            Questions = test.Questions.Select(a => new QuestionModel()
            {
                QuestionId = a.Id,
                Text = a.Content,
                QuestionExistsPhotoUrlElement = a.ImageUrl,
                Instruction = a.Instruction,
                Point = a.Point,
                Answers = a.Answers.Select(g => new AnswerModel()
                {
                    AnswerText = g.Content,
                    AnswerId = g.Id,
                    IsCorrect = g.IsCorrect
                }).ToList()
            }).ToList()
        };
        ViewBag.Subjects = _subjectManager.GetAll().ToList();
        ViewBag.OrtId = test.OrtTestId;
        return View(testModel);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> EditTest([FromForm] TestsModel model)
    {
        W3RootFileManager fileManager = new W3RootFileManager(_env);
        Test test = await _testsManager.GetById(model.TestId);

        var ort = await _ortManager.GetById(test.OrtTestId.Value);

        if (model != null)
        {
            test.Text = model.Text;
            test.OrtInstruction.Description = model.Text;

            if (ort != null)
            {
                ort.TimeForTestInMin -= test.TimeForTestInMin;
                ort.TimeForTestInMin += model.TimeForTestInMin;
                await _ortManager.Update(ort);
            }

            test.TimeForTestInMin = model.TimeForTestInMin;
            test.OrtInstruction.TimeInMin = model.TimeForTestInMin;
            if (!String.IsNullOrEmpty(model.SubjectId.ToString()))
            {
                test.SubjectId = model.SubjectId;
            }

            await _testsManager.Update(test);

            test.Questions.Clear();
            foreach (var question in test.Questions)
            {
                fileManager.DeleteFile(question.ImageUrl);
                question.Answers.Clear();
                await _questionManager.Update(question);
                foreach (var answer in question.Answers)
                {
                    await _answerManager.DeleteById(answer.Id);
                }
                await _questionManager.DeleteById(question.Id);
            }
            foreach (var question in model.Questions)
            {
                string? questionFileName = question.QuestionExistsPhotoUrlElement;
                if (question.Image != null)
                {
                    questionFileName = await fileManager.SaveFormFileAsync("ortTestResources", question.Image);
                }

                Question newQuestion = new Question()
                {
                    Instruction = question.Instruction,
                    Content = question.Text,
                    ImageUrl = questionFileName,
                    IsPublished = true,
                    Point = question.Point,
                    TestId = test.Id
                };

                await _questionManager.Insert(newQuestion);

                foreach (var answer in question.Answers)
                {
                    Answer newAnswer = new Answer()
                    {
                        Content = answer.AnswerText,
                        IsCorrect = answer.IsCorrect,
                        QuestionId = newQuestion.Id
                    };
                    await _answerManager.Insert(newAnswer);
                }
                
            }
            await _testsManager.Update(test);   

            return RedirectToAction("Index");
        }
        return NotFound("Теста не был найден");
    }

    public async Task<IActionResult> DeleteTest(int testId)
    {
        var test = await _testsManager.GetById(testId);
        if (test == null)
        {
            return BadRequest();
        }
        var ort = await _ortManager.GetById(test.OrtTestId.Value);
        if (ort != null)
        {
            ort.TimeForTestInMin -= test.TimeForTestInMin;
            await _ortManager.Update(ort);
        }
        await _testsManager.DeleteById(test.Id);
        return RedirectToAction("Details", new {ortId = ort.Id});
    }
}
