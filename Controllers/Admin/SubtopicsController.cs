using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers.Admin;

[Route("Admin/[controller]/[action]")]
public class SubtopicsController : Controller
{
    private readonly YouCanContext _context;

    public SubtopicsController(YouCanContext context)
    {
        _context = context;
    }

    // GET: Subtopics
    public async Task<IActionResult> Index()
    {
        var youCanContext = _context.Subtopics.Include(s => s.Topic);
        return View(await youCanContext.ToListAsync());
    }

    // GET: Subtopics/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subtopic = await _context.Subtopics
            .Include(s => s.Topic)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subtopic == null)
        {
            return NotFound();
        }

        return View(subtopic);
    }

    // GET: Subtopics/Create
    public IActionResult Create()
    {
        ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name");
        return View();
    }

    // POST: Subtopics/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,TopicId")] Subtopic subtopic)
    {
        if (ModelState.IsValid)
        {
            _context.Add(subtopic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", subtopic.TopicId);
        return View(subtopic);
    }

    // GET: Subtopics/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subtopic = await _context.Subtopics.FindAsync(id);
        if (subtopic == null)
        {
            return NotFound();
        }
        ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", subtopic.TopicId);
        return View(subtopic);
    }

    // POST: Subtopics/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TopicId")] Subtopic subtopic)
    {
        if (id != subtopic.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subtopic);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubtopicExists(subtopic.Id))
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
        ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", subtopic.TopicId);
        return View(subtopic);
    }

    // GET: Subtopics/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subtopic = await _context.Subtopics
            .Include(s => s.Topic)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (subtopic == null)
        {
            return NotFound();
        }

        return View(subtopic);
    }

    // POST: Subtopics/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subtopic = await _context.Subtopics.FindAsync(id);
        if (subtopic != null)
        {
            _context.Subtopics.Remove(subtopic);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubtopicExists(int id)
    {
        return _context.Subtopics.Any(e => e.Id == id);
    }
}
