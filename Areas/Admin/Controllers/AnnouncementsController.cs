using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class AnnouncementsController : Controller
{
	private readonly ICRUDService<Announcement> _announcements;
    private readonly ICRUDService<AdminAction> _adminActions;
    private readonly UserManager<User> _userManager;
    public AnnouncementsController(ICRUDService<Announcement> announcements, 
                                   ICRUDService<AdminAction> adminActions,
                                   UserManager<User> userManager)
    {
        _announcements = announcements;
        _adminActions = adminActions;
        _userManager = userManager;
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

            var admin = await _userManager.GetUserAsync(User);

            var log = new AdminAction()
            {
                UserId = admin.Id,
                Action = "Создание объявления",
                Details = $"{admin.UserName} создал новое объявление {announ.Title}"
            };

            await _adminActions.Update(log);

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

            var admin = await _userManager.GetUserAsync(User);

            var log = new AdminAction()
            {
                UserId = admin.Id,
                Action = "Редактирование объявления",
                Details = $"{admin.UserName} изменил объявление {announ.Title}"
            };

            await _adminActions.Update(log);

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

        var admin = await _userManager.GetUserAsync(User);

        var log = new AdminAction()
        {
            UserId = admin.Id,
            Action = "Удаление объявления",
            Details = $"{admin.UserName} удалил объявление {announ.Title}"
        };

        await _adminActions.Update(log);

        await _announcements.DeleteById(announ.Id);
        return RedirectToAction("Index");
    }
}
