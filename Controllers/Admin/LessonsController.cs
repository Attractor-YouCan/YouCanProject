using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers.Admin;

[Route("Admin/[controller]/{action=index}")]
public class LessonsController : Controller
{
    private readonly YouCanContext _context;

    public LessonsController(YouCanContext context)
    {
        _context = context;
    }

    // GET: Lessons
    public async Task<IActionResult> Index()
    {
        var lesson = _context.Lessons.Include(l => l.Subtopic);
        return View(await lesson.ToListAsync());
    }

    // GET: Lessons/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lesson = await _context.Lessons
            .Include(l => l.Subtopic)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (lesson == null)
        {
            return NotFound();
        }

        return View(lesson);
    }

    // GET: Lessons/Create
    public IActionResult Create()
    {
        ViewData["SubtopicId"] = new SelectList(_context.Subtopics, "Id", "Name");
        return View();
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
        ViewData["SubtopicId"] = new SelectList(_context.Subtopics, "Id", "Name", lesson.SubtopicId);
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
        ViewData["SubtopicId"] = new SelectList(_context.Subtopics, "Id", "Name", lesson.SubtopicId);
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
        ViewData["SubtopicId"] = new SelectList(_context.Subtopics, "Id", "Name", lesson.SubtopicId);
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
            .Include(l => l.Subtopic)
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
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LessonExists(int id)
    {
        return _context.Lessons.Any(e => e.Id == id);
    }
}
