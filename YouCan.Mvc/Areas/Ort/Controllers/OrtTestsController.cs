using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Ort.Controllers;
[Area("Ort")]
[Authorize]
public class OrtTestsController : Controller
{
    private ICrudService<OrtTest> _ortTestService;
    private ICrudService<UserOrtTest> _userOrtTestService;
    private UserManager<User> _userManager;

    public OrtTestsController(ICrudService<OrtTest> ortTestService, 
        ICrudService<UserOrtTest> userOrtTestService,
        UserManager<User> userManager)
    {
        _ortTestService = ortTestService;
        _userOrtTestService = userOrtTestService;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        User? currUser = await _userManager.GetUserAsync(User);
        if (currUser == null)
            RedirectToAction("Login", "Account");
        UserOrtTest? userOrtTest = await _userOrtTestService.GetById(currUser.Id);
        if (userOrtTest == null)
        {
            userOrtTest = new UserOrtTest() { UserId = currUser.Id, IsPassed = false, PassedLevel = 0, OrtTestId = null};
            await _userOrtTestService.Insert(userOrtTest);
        }
        if (userOrtTest.OrtTest == null)
            ViewBag.CurrentOrtTestLevel = 0;
        else
        {
            if (userOrtTest.IsPassed)
                ViewBag.CurrentOrtTestLevel = userOrtTest.OrtTest.OrtLevel;
            else
                ViewBag.CurrentOrtTestLevel = userOrtTest.OrtTest.OrtLevel - 1;
        }
        List<OrtTest> ortTests = _ortTestService.GetAll().ToList();
        return View(ortTests);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int ortTestId)
    {
        OrtTest? ortTest = await _ortTestService.GetById(ortTestId);
        if (ortTest == null)
            return NotFound("no ort test");
        User? curUser = await _userManager.GetUserAsync(User);
        if (curUser == null)
            RedirectToAction("Login", "Account");
        UserOrtTest? userOrtTest = await _userOrtTestService.GetById(curUser.Id);
        int currentLevel = 0;
        if (userOrtTest.OrtTest != null)
        {
            if (userOrtTest.IsPassed)
                currentLevel = (int)userOrtTest.OrtTest.OrtLevel;
            else
                currentLevel = (int)(userOrtTest.OrtTest.OrtLevel - 1);
            if (currentLevel + 1 >= ortTest.OrtLevel)
                return View(ortTest);
        }
        else if(userOrtTest.PassedLevel + 1 >= ortTest.OrtLevel)
            return View(ortTest);
        return RedirectToAction("Index");
    }
}