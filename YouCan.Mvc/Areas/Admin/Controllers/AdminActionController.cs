using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class AdminActionController : Controller
{
    private readonly ICrudService<AdminAction> _adminActions;
    public AdminActionController(ICrudService<AdminAction> adminActions)
    {
        _adminActions = adminActions;
    }
    public IActionResult Index() => View(_adminActions.GetAll().OrderByDescending(x => x.ExecuteTime).ToList());    
}
