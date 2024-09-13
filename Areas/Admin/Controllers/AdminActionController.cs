using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entites.Models;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class AdminActionController : Controller
{
    private readonly ICRUDService<AdminAction> _adminActions;
    public AdminActionController(ICRUDService<AdminAction> adminActions)
    {
        _adminActions = adminActions;
    }
    public IActionResult Index() => View(_adminActions.GetAll().OrderByDescending(x => x.ExecuteTime).ToList());    
}
