using Microsoft.AspNetCore.Mvc;
using YouCan.Areas.ViewModels;
using YouCan.Entities;
using YouCan.Repository;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
public class SubjectController : Controller
{
    private ICRUDService<Subject> _subjectService;
    private readonly IWebHostEnvironment _hostEnvironment;

    public SubjectController(ICRUDService<Subject> subjectService, YouCanContext db, IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _subjectService = subjectService;
    }

    public async Task<IActionResult> Index(int? subSubjectId)
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
    
    public IActionResult Create()
    {
        ViewBag.Subjects = _subjectService.GetAll().ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SubjectViewModel subjectViewModel)
    {
        if (subjectViewModel.ImageFile == null)
        {
            ModelState.AddModelError("ImageFile", "Картинка обязательна для скачивания");
            ViewBag.Subjects = _subjectService.GetAll().ToList();
            return View(subjectViewModel);
        }
        if (ModelState.IsValid)
        {
            Subject subject = new Subject();
            if (subjectViewModel.ImageFile != null && subjectViewModel.ImageFile.Length > 0)
            {
                
                var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "topicImages");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(subjectViewModel.ImageFile.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);
            
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await subjectViewModel.ImageFile.CopyToAsync(fileStream);
                }
            
                subject.ImageUrl = "/topicImages/" + fileName;
            }

            subject.Name = subjectViewModel.Name;
            subject.ParentId = subjectViewModel.ParentId;
            if (subject.ParentId == null)
            {
                subject.SubjectType = SubjectType.Parent;
            }
            else
            {
                subject.SubjectType = SubjectType.Child;
                var parentSubject = _subjectService.GetAll().FirstOrDefault(s => s.Id == subject.ParentId);
                if (parentSubject.SubjectType == SubjectType.Child)
                {
                    parentSubject.SubjectType = SubjectType.Parent;
                    await _subjectService.Update(parentSubject);
                }
            }

            await _subjectService.Insert(subject);
            return RedirectToAction("Index");
        }

        ViewBag.Subjects = _subjectService.GetAll().ToList();
        return View(subjectViewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Subjects = _subjectService.GetAll().ToList();
        var subject = await _subjectService.GetById(id);
        if (subject == null)
        {
            return NotFound();
        }

        SubjectViewModel subjectViewModel = new SubjectViewModel()
        {
            Id = subject.Id,
            Name = subject.Name,
            ParentId = subject.ParentId,
            ImageUrl = subject.ImageUrl,
            SubjectType = subject.SubjectType
        };
        return View(subjectViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(SubjectViewModel subjectViewModel, int id)
    {
        if (ModelState.IsValid)
        {
            var oldSubject = await _subjectService.GetById(id);
            if (subjectViewModel.ParentId != oldSubject.ParentId)
            {
                var subjects = _subjectService.GetAll().Where(s => s.ParentId == oldSubject.ParentId).ToList();
                if (subjects.Count == 1)
                {
                    int oldSubjectParentId = oldSubject.ParentId ?? default(int);
                    var oldParentSubject = await _subjectService.GetById(oldSubjectParentId);
                    if (oldParentSubject != null)
                    {
                        oldParentSubject.SubjectType = SubjectType.Child;
                        await _subjectService.Update(oldParentSubject);
                    }
                }

                int subjectParentId = subjectViewModel.ParentId ?? default(int);
                var newParentSubject = await _subjectService.GetById(subjectParentId);
                if (newParentSubject != null && newParentSubject.SubjectType == SubjectType.Child)
                {
                    newParentSubject.SubjectType = SubjectType.Parent;
                    await _subjectService.Update(newParentSubject);
                }
            }
            if (subjectViewModel.ImageFile != null && subjectViewModel.ImageFile.Length > 0)
            {
                var uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "topicImages");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(subjectViewModel.ImageFile.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await subjectViewModel.ImageFile.CopyToAsync(fileStream);
                }

                oldSubject.ImageUrl = "/topicImages/" + fileName;
            }

            oldSubject.SubjectType = subjectViewModel.SubjectType;
            oldSubject.Name = subjectViewModel.Name;
            oldSubject.ParentId = subjectViewModel.ParentId;
            await _subjectService.Update(oldSubject);
            return RedirectToAction("Index");
        }

        return View(subjectViewModel);
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        
        Subject subject = await _subjectService.GetById(id);
        if (subject != null)
        {
            return View(subject);
        }
        
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(int id)
    {
        if (id != null)
        {
            var subject = await _subjectService.GetById(id);
            var subjects = _subjectService.GetAll().Where(s => s.ParentId == s.ParentId).ToList();
            if (subjects.Count == 1)
            {
                int oldSubjectParentId = subject.ParentId ?? default(int);
                var oldParentSubject = await _subjectService.GetById(oldSubjectParentId);
                if (oldParentSubject != null)
                {
                    oldParentSubject.SubjectType = SubjectType.Child;
                    await _subjectService.Update(oldParentSubject);
                }
            }
            await _subjectService.DeleteById(id);
            return RedirectToAction("Index");
            
        }
        return NotFound();
    }
    
}