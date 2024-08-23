
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Repository;


namespace YouCan.Areas.Rating.Controllers
{
    [Area("Rating")]
    public class LeagueController : Controller
    {
        private readonly LeagueRepository _leagueRepository;

        public LeagueController(LeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        // GET: Rating/League
        public async Task<IActionResult> Index()
        {
            var leagues = _leagueRepository.GetAll().ToList();
            return View(leagues);
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
            var league = await _leagueRepository.Get(id);
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
                    if (await _leagueRepository.Get(league.Id) == null)
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
            var league = await _leagueRepository.Get(id);
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
            var league = await _leagueRepository.Get(id);
            if (league != null)
            {
                await _leagueRepository.Delete(league);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Rating/League/UpdateLeagues
        public async Task<IActionResult> UpdateLeagues()
        {
            await _leagueRepository.UpdateLeaguesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
