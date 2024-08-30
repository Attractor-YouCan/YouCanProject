using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
public class ORTController : Controller
{
    private readonly ICRUDService<OrtTest> _ortManager;
    private readonly ICRUDService<Test> _testsManager;
    private readonly ICRUDService<OrtInstruction> _instructionsManager;
    private readonly UserManager<User> _userManager;
    public ORTController(UserManager<User> userManager, ICRUDService<Test> testManager,
                         ICRUDService<OrtInstruction> instructionsManager, ICRUDService<OrtTest> ortManager)
    {
        _userManager = userManager;
        _testsManager = testManager;
        _instructionsManager = instructionsManager;
        _ortManager = ortManager;
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
}
