﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using YouCan.Entities;
using YouCan.Mvc.ViewModels.Account;
using YouCan.Service;

namespace YouCan.Mvc.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "admin, manager")]
public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly ICrudService<AdminAction> _adminActions;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ICrudService<Tariff> _tariffManager;

    public UsersController(UserManager<User> userManager, 
        IWebHostEnvironment env, 
        ICrudService<AdminAction> adminActions, 
        RoleManager<IdentityRole<int>> roleManager,
        ICrudService<Tariff> tariffManager)
    {
        _userManager = userManager;
        _env = env;
        _adminActions = adminActions;
        _roleManager = roleManager;
        _tariffManager = tariffManager;
    }

    public async Task<IActionResult> Index()
    {
        var users =  _userManager.Users.Include(u => u.Tariff).OrderBy(u => u.Id).ToList();

        ViewBag.Roles =  _roleManager.Roles.ToList();
        ViewBag.Tariffs = _tariffManager.GetAll().ToList();

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