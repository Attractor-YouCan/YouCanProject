using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Mvc;
using YouCan.Repository;
using YouCan.Service.Service;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class UsersController : Controller
{
    private readonly YouCanContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;
    public UsersController(YouCanContext context, UserManager<User> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users.Include(u => u.Tariff)
            .OrderBy(u => u.Id)
            .ToListAsync();

        ViewBag.Roles = await _context.Roles.ToListAsync();
        ViewBag.Tariffs = await _context.Tariffs.ToListAsync();

        return View(users);
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> ChangeRole(int id, string? role)
    {
        if (!String.IsNullOrEmpty(role))
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                await _userManager.AddToRoleAsync(user, role);
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    [Authorize(Roles = "admin, manager")]
    public async Task<IActionResult> ChangeTariff(int id, int? tariffId)
    {
        if (tariffId.HasValue)
        {
            var user = await _context.Users.Include(u => u.Tariff).FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.TariffId = tariffId;
                var tariff = await _context.Tariffs.FindAsync(tariffId);
                if (tariff != null)
                {
                    if (tariff.Duration.HasValue)
                    {
                        user.TariffEndDate = DateTime.UtcNow.AddMonths((int)tariff.Duration);

                        if (await _userManager.IsInRoleAsync(user, "user"))
                        {
                            await _userManager.AddToRoleAsync(user, "prouser");
                            await _userManager.RemoveFromRoleAsync(user, "user");
                        }
                    }
                    else
                    {
                        user.TariffEndDate = null;
                        if (await _userManager.IsInRoleAsync(user, "prouser"))
                        {
                            await _userManager.AddToRoleAsync(user, "user");
                            await _userManager.RemoveFromRoleAsync(user, "prouser");
                        }
                    }
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        return RedirectToAction("Index");
    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Block(int id)
    {
        User user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            var lockUserTask = await _userManager.SetLockoutEnabledAsync(user, true);
            var lockDateTask = await _userManager.SetLockoutEndDateAsync(user, DateTime.MaxValue.ToUniversalTime());

            if (lockUserTask.Succeeded && lockDateTask.Succeeded)
            {
                return RedirectToAction("Index");
            }
        }
        return NotFound();
    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Unblock(int id)
    {
        User user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            var lockDateTask = await _userManager.SetLockoutEndDateAsync(user, (DateTime.UtcNow - TimeSpan.FromDays(1)).ToUniversalTime());
            var lockUserTask = await _userManager.SetLockoutEnabledAsync(user, false);

            if (lockUserTask.Succeeded && lockDateTask.Succeeded)
            {
                return RedirectToAction("Index");
            }
        }
        return NotFound();
    }
    public async Task<IActionResult> Details(int id)
    {
        User user = await _context.Users
            .Include(u => u.Lessons)
            .ThenInclude(l => l.Lesson)
            .Include(u => u.Tests)
            .ThenInclude(l => l.OrtTest)
            .Include(u => u.Questions)
            .Include(u => u.Statistic)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user != null)
        {
            return View(user);
        }
        return NotFound();
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(RegisterViewModel model, IFormFile? uploadedFile)
    {
        if (ModelState.IsValid)
        {
            string path = "/userImages/defProf-ProfileN=1.png";
            if (uploadedFile != null)
            {
                string newFileName = Path.ChangeExtension($"{model.UserName.Trim()}-ProfileN=1", Path.GetExtension(uploadedFile.FileName));
                path = $"/userImages/" + newFileName.Trim();
                using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }

            User user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                FullName = model.LastName + " " + model.FirstName,
                AvatarUrl = path,
                BirthDate = model.BirthDate.ToUniversalTime(),
                CreatedAt = DateTime.UtcNow.AddHours(6),
                Disctrict = model.District
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _userManager.SetLockoutEnabledAsync(user, false);
                var startTariff = _context.Tariffs.FirstOrDefault(t => t.Name == "Start");
                user.TariffId = startTariff.Id;
                user.TariffEndDate = null;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        ModelState.AddModelError("", "Something went wrong! Please check all info");
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user != null)
        {
            return View(user);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Edit(User user, int id, string? currentPassword, string? newPassword)
    {
        var oldUser = await _userManager.FindByIdAsync(id.ToString());
        if (oldUser != null)
        {
            oldUser.Email = user.Email;
            oldUser.UserName = user.UserName;
            oldUser.PhoneNumber = user.PhoneNumber;
            oldUser.FullName = user.FullName;
            oldUser.BirthDate = user.BirthDate;

            if (oldUser.BirthDate.HasValue)
            {
                oldUser.BirthDate = ((DateTime)oldUser.BirthDate).ToUniversalTime();
            }
            if (currentPassword != null && newPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return NotFound();
                }
            }
            await _userManager.UpdateAsync(oldUser);
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("", "Пользователь не найден");
        return NotFound();
    }
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Remove(user);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}