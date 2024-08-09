using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
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
        var tests = _context.Tests.Include(t => t.Lesson);
        return View(await tests.ToListAsync());
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
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
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
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title");
        return View();
    }

    // POST: Tests/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Test test)
    {
        if (test.MinutesForTest != null)
        {
            test.TimeForTestInMin = TimeSpan.FromMinutes(test.MinutesForTest.Value).Minutes;
        }
        if (ModelState.IsValid)
        {
            _context.Add(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title", test.LessonId);
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
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title", test.LessonId);
        return View(test);
    }

    // POST: Tests/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Test test)
    {
        if (id != test.Id)
        {
            return NotFound();
        }
        if (test.MinutesForTest != null)
        {
            test.TimeForTestInMin = TimeSpan.FromMinutes(test.MinutesForTest.Value).Minutes;
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
        ViewData["LessonId"] = new SelectList(_context.Lessons, "Id", "Title", test.LessonId);
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
