using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;
using YouCan.Services;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
public class SubjectController : Controller
{
    private readonly YouCanContext _db;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SubjectController(YouCanContext db, IWebHostEnvironment hostEnvironment)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<IActionResult> Index(int? subSubjectId)
    {
        List<Subject> subjects = new List<Subject>();
        if (subSubjectId == null)
        {
            subjects = await _db.Subjects.Where(s => s.ParentId == null).ToListAsync();
        }
        else
        {
            subjects = await _db.Subjects.Where(s => s.ParentId == subSubjectId).ToListAsync();
        }
        return View(subjects);
    }
    
    public IActionResult Create()
    {
        ViewBag.Subjects = _db.Subjects.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Subject subject)
    {
        if (subject.ImageFile == null)
        {
            ModelState.AddModelError("ImageFile", "Картинка обязательна для скачивания");
            ViewBag.Subjects = _db.Subjects.ToList();
            return View(subject);
        }
        if (ModelState.IsValid)
        {
            if (subject.ImageFile != null && subject.ImageFile.Length > 0)
            {
                
                var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "topicImages");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(subject.ImageFile.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);
            
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await subject.ImageFile.CopyToAsync(fileStream);
                }
            
                subject.ImageUrl = "/topicImages/" + fileName;
            }

            if (subject.ParentId == null)
            {
                subject.SubjectType = SubjectType.Parent;
            }
            else
            {
                subject.SubjectType = SubjectType.Child;
                var parentSubject = await _db.Subjects.FirstOrDefaultAsync(s => s.Id == subject.ParentId);
                if (parentSubject.SubjectType == SubjectType.Child)
                {
                    parentSubject.SubjectType = SubjectType.Parent;
                    _db.Update(parentSubject);
                }
            }

            await _db.AddAsync(subject);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return BadRequest();
    }
    
}