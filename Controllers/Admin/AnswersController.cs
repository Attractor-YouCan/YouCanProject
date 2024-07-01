using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers.Admin;

[Route("Admin/[controller]/{action=index}")]
public class AnswersController : Controller
{
    private readonly YouCanContext _context;

    public AnswersController(YouCanContext context)
    {
        _context = context;
    }

    // GET: Answers
    public async Task<IActionResult> Index()
    {
        var answers = _context.Answers.Include(a => a.Question);
        return View(await answers.ToListAsync());
    }

    // GET: Answers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var answer = await _context.Answers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (answer == null)
        {
            return NotFound();
        }

        return View(answer);
    }

    // GET: Answers/Create
    public async Task<IActionResult> Create(int id, string? returnUrl)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question != null)
        {
            ViewBag.QuestionId = question.Id;    
            ViewBag.ReturnUrl = returnUrl;
        }
        return View();
    }

    // POST: Answers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int id, Answer answer, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            _context.Add(answer);
            await _context.SaveChangesAsync();
            if (returnUrl != null && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }
        var question = await _context.Questions.FindAsync(id);
        if (question != null)
        {
            ViewBag.QuestionId = question.Id;
        }
        return View(answer);
    }

    // GET: Answers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var answer = await _context.Answers.FindAsync(id);
        if (answer == null)
        {
            return NotFound();
        }
        ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
        return View(answer);
    }

    // POST: Answers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Text,IsCorrect,QuestionId")] Answer answer)
    {
        if (id != answer.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(answer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(answer.Id))
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
        ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Id", answer.QuestionId);
        return View(answer);
    }

    // GET: Answers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var answer = await _context.Answers
            .Include(a => a.Question)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (answer == null)
        {
            return NotFound();
        }

        return View(answer);
    }

    // POST: Answers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var answer = await _context.Answers.FindAsync(id);
        if (answer != null)
        {
            _context.Answers.Remove(answer);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AnswerExists(int id)
    {
        return _context.Answers.Any(e => e.Id == id);
    }
}
