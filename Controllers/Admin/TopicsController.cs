using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers.Admin;

public class TopicsController : Controller
{
    private readonly YouCanContext _context;

    public TopicsController(YouCanContext context)
    {
        _context = context;
    }

    // GET: Topics
    public async Task<IActionResult> Index()
    {
        return View(await _context.Topics.ToListAsync());
    }

    // GET: Topics/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics
            .Include(t => t.Subtopics)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (topic == null)
        {
            return NotFound();
        }

        return View(topic);
    }

    // GET: Topics/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Topics/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Topic topic)
    {
        if (ModelState.IsValid)
        {
            _context.Add(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(topic);
    }

    // GET: Topics/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics.FindAsync(id);
        if (topic == null)
        {
            return NotFound();
        }
        return View(topic);
    }

    // POST: Topics/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Topic topic)
    {
        if (id != topic.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(topic);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(topic.Id))
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
        return View(topic);
    }

    // GET: Topics/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var topic = await _context.Topics
            .FirstOrDefaultAsync(m => m.Id == id);
        if (topic == null)
        {
            return NotFound();
        }

        return View(topic);
    }

    // POST: Topics/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic != null)
        {
            _context.Topics.Remove(topic);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TopicExists(int id)
    {
        return _context.Topics.Any(e => e.Id == id);
    }
}
