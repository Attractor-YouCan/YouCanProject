using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers.Admin;

[Route("Admin/[controller]/[action]")]
public class TestsController : Controller
{
    private readonly YouCanContext _context;

    public TestsController(YouCanContext context)
    {
        _context = context;
    }

    // GET: Tests
    public async Task<IActionResult> Index()
    {
        var youCanContext = _context.Tests.Include(t => t.Lesson);
        return View(await youCanContext.ToListAsync());
    }

    // GET: Tests/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var test = await _context.Tests
            .Include(t => t.Lesson)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (test == null)
        {
            return NotFound();
        }

        return View(test);
    }

    // GET: Tests/Create
    public IActionResult Create()
    {
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id");
        return View();
    }

    // POST: Tests/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,GainingExperience,TimeForTest,LessonId")] Test test)
    {
        if (ModelState.IsValid)
        {
            _context.Add(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", test.LessonId);
        return View(test);
    }

    // GET: Tests/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var test = await _context.Tests.FindAsync(id);
        if (test == null)
        {
            return NotFound();
        }
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", test.LessonId);
        return View(test);
    }

    // POST: Tests/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,GainingExperience,TimeForTest,LessonId")] Test test)
    {
        if (id != test.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(test);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(test.Id))
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
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Id", test.LessonId);
        return View(test);
    }

    // GET: Tests/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var test = await _context.Tests
            .Include(t => t.Lesson)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (test == null)
        {
            return NotFound();
        }

        return View(test);
    }

    // POST: Tests/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var test = await _context.Tests.FindAsync(id);
        if (test != null)
        {
            _context.Tests.Remove(test);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TestExists(int id)
    {
        return _context.Tests.Any(e => e.Id == id);
    }
}
