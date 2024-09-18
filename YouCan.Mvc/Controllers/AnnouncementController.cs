using Microsoft.AspNetCore.Mvc;
using YouCan.Entites.Models;
using YouCan.Service.Service;

namespace YouCan.Controllers;

public class AnnouncementController : Controller
{
    private readonly ICrudService<Announcement> _annoucements;
    public AnnouncementController(ICrudService<Announcement> announcements)
    {
        _annoucements = announcements;
    }
    public IActionResult Index() => View(_annoucements.GetAll().ToList());    
}
