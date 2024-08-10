using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Repository;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
public class LessonsController : Controller
{
    private readonly YouCanContext _context;
    private ICRUDService<Subject> _subjectService;

    public LessonsController(YouCanContext context, ICRUDService<Subject> subjectService)
    {
        _context = context;
        _subjectService = subjectService;
    }

    // GET: Lessons
    public async Task<IActionResult> Index(int subSubjectId)
    {
        if (subSubjectId != null)
        {
            var lesson = _context.Lessons.Include(l => l.Subject)
                .Where(l => l.SubjectId == subSubjectId);
            ViewBag.SubjectId = subSubjectId;
            return View(await lesson.ToListAsync());
        }

        return NotFound();
    }
    
    public async Task<IActionResult> IndexOfSubjects(int? subSubjectId)
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

    // GET: Lessons/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lesson == null)
        {
            return NotFound();
        }

        return View(lesson);
    }

    // GET: Lessons/Create
    public async Task<IActionResult> Create(int subjectId)
    {
        var subject = await _subjectService.GetById(subjectId);
        return View(new Lesson(){Subject = subject});
    }

    // POST: Lessons/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lesson lesson)
    {
        if (ModelState.IsValid)
        {
            _context.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(s => s.SubjectType == SubjectType.Child), "Id", "Name", lesson.SubjectId);
        return View(lesson);
    }

    // GET: Lessons/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null)
        {
            return NotFound();
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(s => s.SubjectType == SubjectType.Child), "Id", "Name", lesson.SubjectId);
        return View(lesson);
    }

    // POST: Lessons/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Lesson lesson)
    {
        if (id != lesson.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(lesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(lesson.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["SubjectId"] = new SelectList(_context.Subjects.Where(s => s.SubjectType == SubjectType.Child), "Id", "Name", lesson.SubjectId);
        return View(lesson);
    }

    // GET: Lessons/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons
            .Include(l => l.Subject)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lesson == null)
        {
            return NotFound();
        }

        return View(lesson);
    }

    // POST: Lessons/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson != null)
        {
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool LessonExists(int id)
    {
        return _context.Lessons.Any(e => e.Id == id);
    }
}
