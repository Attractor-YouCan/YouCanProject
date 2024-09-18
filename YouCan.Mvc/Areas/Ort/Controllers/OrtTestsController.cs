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
        UserOrtTest? userOrtTest = _userOrtTestService.GetAll().FirstOrDefault(t => t.UserId == currUser.Id);
        if (userOrtTest == null)
        {
            userOrtTest = new UserOrtTest()
            {
                UserId = currUser.Id, 
                IsPassed = false, 
                PassedLevel = 0, 
                OrtTestId = null
            };
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

        var userRoles = await _userManager.GetRolesAsync(currUser);
        ViewBag.UserRoles = userRoles;
        
        List<OrtTest> ortTests = _ortTestService.GetAll().ToList();
        return View(ortTests);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int ortTestId)
    {
        OrtTest? ortTest = await _ortTestService.GetById(ortTestId);
        if (ortTest == null)
            return NotFound("no ort test");
        return View(ortTest);
    }
}