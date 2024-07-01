using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;
using static System.Net.Mime.MediaTypeNames;

namespace YouCan.Controllers.Admin;

[Route("Admin/[controller]/{action=index}")]
public class QuestionsController : Controller
{
    private readonly YouCanContext _context;

    public QuestionsController(YouCanContext context)
    {
        _context = context;
    }

    // GET: Questions
    public async Task<IActionResult> Index()
    {
        var questions = _context.Questions.Include(q => q.Test).Include(q => q.User);
        return View(await questions.ToListAsync());
    }

    // GET: Questions/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions
            .Include(q => q.Test)
            .Include(q => q.User)
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // GET: Questions/Create
    public async Task<IActionResult> Create(int id, string? returnUrl)
    {
        var test = await _context.Tests.FindAsync(id);
        if (test != null)
        {
            ViewBag.TestId = test.Id;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        return NotFound();
    }

    // POST: Questions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int id, Question question, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            _context.Add(question);
            await _context.SaveChangesAsync();
            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }
        var test = await _context.Tests.FindAsync(id);
        if (test != null)
        {
            ViewBag.TestId = test.Id;
            return View(question);
        }
        return NotFound();
    }

    // GET: Questions/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions.FindAsync(id);
        if (question == null)
        {
            return NotFound();
        }
        ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", question.TestId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
        return View(question);
    }

    // POST: Questions/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Type,IsPublished,TestId,UserId")] Question question)
    {
        if (id != question.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(question);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.Id))
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
        ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Id", question.TestId);
        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", question.UserId);
        return View(question);
    }

    // GET: Questions/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var question = await _context.Questions
            .Include(q => q.Test)
            .Include(q => q.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // POST: Questions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question != null)
        {
            _context.Questions.Remove(question);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool QuestionExists(int id)
    {
        return _context.Questions.Any(e => e.Id == id);
    }
}
