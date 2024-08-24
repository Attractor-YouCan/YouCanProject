using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Study.Controllers;

[Area("Study")]
[Authorize]
public class SubTopicsController : Controller
{
    private ICrudService<Subject> _subjectService;
    private UserManager<User> _userManager;

    public SubTopicsController(ICrudService<Subject> subjectService,
        UserManager<User> userManager)
    {
        _subjectService = subjectService;
        _userManager = userManager;
    }

    public IActionResult Index(int? subSubjectId)
    {
        List<Subject> subjects = new List<Subject>();
        if (subSubjectId == null)
        {
            subjects = _subjectService.GetAll().Where(s => s.ParentId == null).ToList();
        }
        else
        {
            subjects = _subjectService.GetAll().Where(s => s.ParentId == subSubjectId).ToList();
        }
        return View(subjects);
    }

}