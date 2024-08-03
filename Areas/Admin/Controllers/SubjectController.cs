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
        ViewBag.Subjects = _db.Subjects.ToList();
        return View(subject);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Subjects = _db.Subjects.ToList();
        var subject = await _db.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }
        return View(subject);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Subject subject, int id)
    {
        var oldSubject = await _db.Subjects.FindAsync(id);
        if (subject.ParentId != oldSubject.ParentId)
        {
            var subjects = _db.Subjects.Where(s => s.ParentId == oldSubject.ParentId).ToList();
            if (subjects.Count == 1)
            {
                var oldParentSubject = await _db.Subjects.FindAsync(oldSubject.ParentId);
                if (oldParentSubject != null)
                {
                    oldParentSubject.SubjectType = SubjectType.Child;
                    _db.Update(oldParentSubject);
                }
            }

            var newParentSubject = await _db.Subjects.FindAsync(subject.ParentId);
            if (newParentSubject != null && newParentSubject.SubjectType == SubjectType.Child)
            {
                newParentSubject.SubjectType = SubjectType.Parent;
                _db.Update(newParentSubject);
            }
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
            else
            {
                subject.ImageUrl = oldSubject.ImageUrl;
            }

            subject.SubjectType = oldSubject.SubjectType;

            _db.Entry(oldSubject).CurrentValues.SetValues(subject);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(subject);
    }
    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id != null)
        {
            Subject? subject = await _db.Subjects.FirstOrDefaultAsync(p => p.Id == id);
            if (subject != null)
            {
                return View(subject);
            }
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int? id)
    {
        if (id != null)
        {
            Subject? subject = await _db.Subjects.FirstOrDefaultAsync(p => p.Id == id);
            if (subject != null)
            {
                _db.Subjects.Remove(subject);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }
        return NotFound();
    }
    
}