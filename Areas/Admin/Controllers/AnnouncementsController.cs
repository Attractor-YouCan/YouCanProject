using Microsoft.AspNetCore.Mvc;
using YouCan.Entites.Models;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
public class AnnouncementsController : Controller
{
	private readonly ICRUDService<Announcement> _announcements;
    public AnnouncementsController(ICRUDService<Announcement> announcements)
    {
        _announcements = announcements;
    }
    public IActionResult Index() => View(_announcements.GetAll().OrderBy(e => e.Id).ToList());
    [HttpGet]
    public IActionResult Create() => View();
    [HttpPost]
    public async Task<IActionResult> Create(Announcement announ)
    {
        if (ModelState.IsValid)
        {
            await _announcements.Insert(announ);
            return RedirectToAction("Index");
        }
        return NotFound();
    }
    public async Task<IActionResult> Details(int announId)
    {
        var announ = await _announcements.GetById(announId);
        
        if (announ == null)
        {
            return NotFound();
        }

        return View(announ);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int announId)
    {
        var announ = await _announcements.GetById(announId);

        if (announ == null)
        {
            return NotFound();
        }
        ViewBag.AnnounId = announId;
        return View(announ);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Announcement announ)
    {
        if (ModelState.IsValid)
        {
            await _announcements.Update(announ);
            return RedirectToAction("Index");
        }
        return View(announ);
    }
    public async Task<IActionResult> Delete(int announId)
    {
        var announ = await _announcements.GetById(announId);

        if (announ == null)
        {
            return NotFound();
        }

        await _announcements.DeleteById(announ.Id);
        return RedirectToAction("Index");
    }
}
