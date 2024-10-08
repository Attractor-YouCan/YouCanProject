using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Rating.Controllers;

[Area("Rating")]
public class LeagueController : Controller
{
    private readonly ICrudService<League> _leagueRepository;
    //private readonly LeagueRepository _leagueRepository;
    private UserManager<User> _userManager;

    public LeagueController(ICrudService<League> leagueRepository, UserManager<User> userManager)
    {
        _leagueRepository = leagueRepository;
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
        var leagues = _leagueRepository.GetAll().ToList();
        return View(leagues);
    }
    

    private async  Task<List<User>> GetUserLeaderboardLeague(string leagueName)
    {
        if (leagueName == null)
        {
            return null;
        }
        var leagueRepo = _leagueRepository.GetAll()
            .FirstOrDefault(l => l.LeagueName.ToLower().Trim() == leagueName.ToLower().Trim());
        if (leagueRepo == null)
        {
            return null;
        }
        var users =  _userManager.Users
            .Include(u => u.League)
            .OrderByDescending(u => u.Rank)
            .ToList();

        var leagueUsers = new List<User>();
        foreach (var user in users)
        {
                if (user.UserLessonScore >= leagueRepo.MinPoints && user.UserLessonScore <= leagueRepo.MaxPoints)
                {
                    leagueUsers.Add(user);
                }
        }
        
        return leagueUsers;
    }
    
    [HttpGet]
    public async Task<JsonResult> GetUsersByLeague(string leagueName)
    {
        var users = await GetUserLeaderboardLeague(leagueName);

        if (users == null)
        {
            return Json(new List<object>()); 
        }

        var result = users.Select(user => new
        {
            user.Id,
            user.UserName,
            user.FullName,
            user.AvatarUrl,
            user.Rank,
            user.UserLessonScore
        }).ToList();

        return Json(result);
    }


    // GET: Rating/League/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Rating/League/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LeagueName,MinPoints,MaxPoints,StartDate,EndDate")] League league)
    {
        if (ModelState.IsValid)
        {
            await _leagueRepository.Insert(league);
            return RedirectToAction(nameof(Index));
        }
        return View(league);
    }

    // GET: Rating/League/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var league = await _leagueRepository.GetById(id);
        if (league == null)
        {
            return NotFound();
        }
        return View(league);
    }

    // POST: Rating/League/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LeagueId,LeagueName,MinPoints,MaxPoints,StartDate,EndDate")] League league)
    {
        if (id != league.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _leagueRepository.Update(league);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _leagueRepository.GetById(league.Id) == null)
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
        return View(league);
    }

    // GET: Rating/League/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var league = await _leagueRepository.GetById(id);
        if (league == null)
        {
            return NotFound();
        }

        return View(league);
    }

    // POST: Rating/League/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var league = await _leagueRepository.GetById(id);
        if (league != null)
        {
            await _leagueRepository.DeleteById(id);
        }

        return RedirectToAction(nameof(Index));
    }
    
}
