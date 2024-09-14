using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using YouCan.Areas.Admin.Services;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Mvc;
using YouCan.Repository;
using YouCan.Service.Service;
using static YouCan.Areas.Admin.Services.UserSortOrder;

namespace YouCan.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly ICRUDService<AdminAction> _adminActions;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ICRUDService<Tariff> _tariffManager;

    public UsersController(UserManager<User> userManager, 
        IWebHostEnvironment env, 
        ICRUDService<AdminAction> adminActions, 
        RoleManager<IdentityRole<int>> roleManager,
        ICRUDService<Tariff> tariffManager)
    {
        _userManager = userManager;
        _env = env;
        _adminActions = adminActions;
        _roleManager = roleManager;
        _tariffManager = tariffManager;
    }

    public async Task<IActionResult> Index(UserSortOrder order = IdAsc, int page = 1)
    {
        int pageSize = 10;

        ViewData["IdSort"] = order == IdAsc ? IdDesc : IdAsc;
        ViewData["NameSort"] = order == NameAsc ? NameDesc : NameAsc;
        ViewData["EmailSort"] = order == EmailAsc ? EmailDesc : EmailAsc;
        ViewData["RoleSort"] = order == RoleAsc ? RoleDesc : RoleAsc;
        ViewData["TariffSort"] = order == TariffAsc ? TariffDesc : TariffAsc;
        ViewData["TariffEndDateSort"] = order == TariffEndDateAsc ? TariffEndDateDesc : TariffEndDateAsc;
        ViewData["ActiveSort"] = order == ActiveAsc ? ActiveDesc : ActiveAsc;
        ViewData["LessonsCountSort"] = order == LessonsCountAsc ? LessonsCountDesc : LessonsCountAsc;
        ViewData["LessonsFinishedCountSort"] = order == LessonsFinishedCountAsc ? LessonsFinishedCountDesc : LessonsFinishedCountAsc;


        var users =  _userManager.Users
            .Include(u => u.Tariff)
            .Include(u => u.Statistic)
            .Include(u => u.Lessons)
            .Include(u => u.Tests)
            .OrderBy(u => u.Id)
            .ToList();

        users = SortUsers(users, order);

        var totalUsers = users.Count;
        users = users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.Roles =  _roleManager.Roles.ToList();
        ViewBag.Tariffs = _tariffManager.GetAll().ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
        ViewData["CurrentSort"] = order;

        return View(users);
    }
    [NonAction]
    public List<User> SortUsers(List<User> users, UserSortOrder order)
    {
        users = order switch
        {
            IdAsc => users.OrderBy(u => u.Id).ToList(),
            IdDesc => users.OrderByDescending(u => u.Id).ToList(),

            NameAsc => users.OrderBy(u => u.UserName).ToList(),
            NameDesc => users.OrderByDescending(u => u.UserName).ToList(),

            EmailAsc => users.OrderBy(u => u.Email).ToList(),
            EmailDesc => users.OrderByDescending(u => u.Email).ToList(),

            RoleAsc => users.OrderBy(u => _userManager.GetRolesAsync(u).Result.FirstOrDefault()).ToList(),
            RoleDesc => users.OrderByDescending(u => _userManager.GetRolesAsync(u).Result.FirstOrDefault()).ToList(),

            TariffAsc => users.OrderBy(u => u.TariffId).ToList(),
            TariffDesc => users.OrderByDescending(u => u.TariffId).ToList(),

            TariffEndDateAsc => users.OrderBy(u => u.TariffEndDate).ToList(),
            TariffEndDateDesc => users.OrderByDescending(u => u.TariffEndDate).ToList(),

            ActiveAsc => users.OrderByDescending(u => !(u.LockoutEnabled && u.LockoutEnd != null)).ToList(),
            ActiveDesc => users.OrderByDescending(u => u.LockoutEnabled && u.LockoutEnd != null).ToList(),

            LessonsCountAsc => users.OrderBy(u => u.Lessons.Count).ToList(),
            LessonsCountDesc => users.OrderByDescending(u => u.Lessons.Count).ToList(),

            LessonsFinishedCountAsc => users.OrderBy(u => u.Lessons.Count(l => l.IsPassed)).ToList(),
            LessonsFinishedCountDesc => users.OrderByDescending(u => u.Lessons.Count(l => l.IsPassed)).ToList(),

            _ => users.OrderBy(u => u.Id).ToList(), // Default sorting
        };
        return users;
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
                var userRole = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRole);
                await _userManager.AddToRoleAsync(user, role);

                var admin = await _userManager.GetUserAsync(User);

                var log = new AdminAction
                {
                    UserId = admin.Id,
                    Action = "Изменение прав доступа",
                    ExecuteTime = DateTime.UtcNow,
                    Details = $"{admin.UserName} изменил роль пользователя {user.UserName} с {userRole[0].ToString()} на {role}"
                };
                await _adminActions.Insert(log);
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
            var user =  _userManager.Users.Include(u => u.Tariff).FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.TariffId = tariffId;
                var tariff = await _tariffManager.GetById((int)tariffId); //_context.Tariffs.FindAsync(tariffId);
                var admin = await _userManager.GetUserAsync(User);
                if (tariff != null)
                {
                    if (tariff.Duration.HasValue)
                    {
                        user.TariffEndDate = DateTime.UtcNow.AddMonths((int)tariff.Duration);
                        user.TariffStartDate = DateTime.UtcNow;
                        if (await _userManager.IsInRoleAsync(user, "user"))
                        {
                            await _userManager.AddToRoleAsync(user, "prouser");
                            await _userManager.RemoveFromRoleAsync(user, "user");
                        }
                    }
                    else
                    {
                        user.TariffEndDate = null;
                        user.TariffStartDate = DateTime.UtcNow;
                        if (await _userManager.IsInRoleAsync(user, "prouser"))
                        {
                            await _userManager.AddToRoleAsync(user, "user");
                            await _userManager.RemoveFromRoleAsync(user, "prouser");
                        }
                    }
                    var log = new AdminAction
                    {
                        UserId = admin.Id,
                        Action = "Изменение тарифа",
                        ExecuteTime = DateTime.UtcNow,
                        Details = $"{admin.UserName} изменил тариф пользователя {user.UserName} с {user.Tariff.Name} на {tariff.Name}"
                    };
                    await _adminActions.Insert(log);
                }

                await _userManager.UpdateAsync(user); //_context.Update(user);
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

            var admin = await _userManager.GetUserAsync(User);

            if (lockUserTask.Succeeded && lockDateTask.Succeeded)
            {
                var log = new AdminAction
                {
                    UserId = admin.Id,
                    Action = "Блокировка пользователя",
                    ExecuteTime = DateTime.UtcNow,
                    Details = $"{admin.UserName} заблокировал пользователя {user.UserName}"
                };
                await _adminActions.Insert(log);
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
            var lockDateTask = await _userManager.SetLockoutEndDateAsync(user, null);
            var lockUserTask = await _userManager.SetLockoutEnabledAsync(user, false);

            var admin = await _userManager.GetUserAsync(User);

            if (lockUserTask.Succeeded && lockDateTask.Succeeded)
            {
                var log = new AdminAction
                {
                    UserId = admin.Id,
                    Action = "Разблокировка пользователя",
                    ExecuteTime = DateTime.UtcNow,
                    Details = $"{admin.UserName} разблокировал пользователя {user.UserName}"
                };
                await _adminActions.Insert(log);
                return RedirectToAction("Index");
            }
        }
        return NotFound();
    }
    public async Task<IActionResult> Details(int id)
    {
        User user = await _userManager.Users
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

            var admin = await _userManager.GetUserAsync(User);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _userManager.SetLockoutEnabledAsync(user, false);

                var startTariff = _tariffManager.GetAll().FirstOrDefault(t => t.Name == "Start"); //_context.Tariffs.FirstOrDefault(t => t.Name == "Start");
                user.TariffId = startTariff.Id;
                user.TariffStartDate = DateTime.UtcNow; 
                user.TariffEndDate = null;

                await _userManager.UpdateAsync(user);  //_context.Update(user);

                var log = new AdminAction
                {
                    UserId = admin.Id,
                    Action = "Создание пользователя",
                    ExecuteTime = DateTime.UtcNow,
                    Details = $"{admin.UserName} создал пользователя {user.UserName}"
                };
                await _adminActions.Insert(log);

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

            var admin = await _userManager.GetUserAsync(User);

            var log = new AdminAction
            {
                UserId = admin.Id,
                Action = "Редактирование пользователя",
                ExecuteTime = DateTime.UtcNow,
                Details = $"{admin.UserName} отредактировал пользователя {user.UserName}"
            };
            await _adminActions.Insert(log);

            await _userManager.UpdateAsync(oldUser);
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("", "Пользователь не найден");
        return NotFound();
    }
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString()); //await _context.Users.FindAsync(id);
        var admin = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var log = new AdminAction
            {
                UserId = admin.Id,
                Action = "Удаление пользователя",
                ExecuteTime = DateTime.UtcNow,
                Details = $"{admin.UserName} удалил пользователя {user.UserName}"
            };
            await _adminActions.Insert(log);
            await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }
}