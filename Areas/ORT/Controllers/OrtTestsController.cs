using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Areas.ORT.Controllers;
[Area("ORT")]
[Authorize]
public class OrtTestsController : Controller
{
    private YouCanContext _db;
    private UserManager<User> _userManager;

    public OrtTestsController(YouCanContext db, UserManager<User> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        User? currUser = await _userManager.GetUserAsync(User);
        if (currUser == null)
            RedirectToAction("Login", "Account");
        UserOrtTest? userOrtTest = await _db.UserORTTests
            .Include(t => t.OrtTest)
            .FirstOrDefaultAsync(u => u.UserId == currUser.Id);
        if (userOrtTest == null)
        {
            userOrtTest = new UserOrtTest() { UserId = currUser.Id, IsPassed = false, PassedLevel = 0, OrtTestId = null};
            _db.UserORTTests.Add(userOrtTest);
            await _db.SaveChangesAsync();
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
        List<OrtTest> ortTests = await _db.OrtTests.ToListAsync();
        return View(ortTests);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int ortTestId)
    {
        OrtTest? ortTest = await _db.OrtTests
            .Include(t => t.Tests)
                .ThenInclude(s => s.Subject)
            .Include(t => t.Tests)
                .ThenInclude(s => s.OrtInstruction)
            .FirstOrDefaultAsync(t => t.Id == ortTestId);
        if (ortTest == null)
            return NotFound("no ort test");
        User? curUser = await _userManager.GetUserAsync(User);
        if (curUser == null)
            RedirectToAction("Login", "Account");
        UserOrtTest? userOrtTest = await _db.UserORTTests
            .Include(t => t.OrtTest)
            .FirstOrDefaultAsync(t => t.UserId == curUser.Id);
        int currentLevel = 0;
        if (userOrtTest.IsPassed)
            currentLevel = (int)userOrtTest.OrtTest.OrtLevel;
        else
            currentLevel = (int)(userOrtTest.OrtTest.OrtLevel - 1);
        if (currentLevel + 1 >= ortTest.OrtLevel)
            return View(ortTest);
        return RedirectToAction("Index");
    }
}