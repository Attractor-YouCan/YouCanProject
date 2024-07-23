using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

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
                .ThenInclude(t => t.Questions)
                    .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == ortTestId);
        if (ortTest == null)
            return NotFound("no ort test");
        UserOrtTest? userOrtTest = await _db.UserORTTests
            .Include(t => t.OrtTest)
            .FirstOrDefaultAsync(t => t.UserId == curUser.Id);
        if (userOrtTest.PassedLevel + 1 < ortTest.OrtLevel)
            return RedirectToAction("Index", "OrtTests");
        return View(ortTest.Tests);
    }
}