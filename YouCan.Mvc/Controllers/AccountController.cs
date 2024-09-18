using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Mvc.Services.Email;
using YouCan.Mvc.ViewModels.Account;
using YouCan.Service;
using YouCan.Entities;
using Razor.Templating.Core;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace YouCan.Mvc.Controllers;
public class AccountController : Controller
{
    private IUserCrud _userService;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private IWebHostEnvironment _environment;
    private ICrudService<UserLevel> _userLevel;
    private ICrudService<UserExperience> _userExperiance;
    private ICrudService<UserLessons> _userLessonService;
    private readonly TwoFactorService _twoFactorService;
    private readonly ICrudService<Tariff> _tariffs;
    private readonly IStringLocalizer _localizer;
    private readonly IRazorTemplateEngine _razorTemplateEngine;

    public AccountController(IUserCrud userService, 
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IWebHostEnvironment environment,
        TwoFactorService twoFactorService,
        ICrudService<UserLevel> userLevel,
        ICrudService<UserLessons> userLessonService,
        ICrudService<Tariff> tariffs,
        ICrudService<UserExperience> userExperiance,
        IStringLocalizer<AccountController> localizer,
        IRazorTemplateEngine razorTemplateEngine)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _environment = environment;
        _twoFactorService = twoFactorService;
        _userLevel = userLevel;
        _userLessonService = userLessonService;
        _tariffs = tariffs;
        _localizer = localizer;
        _userExperiance = userExperiance;
        _razorTemplateEngine = razorTemplateEngine;
    }

    [Authorize]
    public async Task<IActionResult> Profile(int? userId)
    {
        User user;
        if (!userId.HasValue)
            user = await _userService.GetById(int.Parse(_userManager.GetUserId(User)));
        else
            user = await _userService.GetById((int)userId);

        if (user != null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var adminUser = await _userManager.IsInRoleAsync(currentUser, "admin");
            var isOwner = currentUser.Id == user.Id;

            var userLevels = _userLevel.GetAll()
                .Where(l => l.UserId == currentUser.Id)
                .GroupBy(l => l.SubjectId)
                .Select(g => g.OrderByDescending(l => l.Level).FirstOrDefault())
                .ToList();

            var userLessons = _userLessonService.GetAll()
                .Where(l => l.UserId == currentUser.Id && l.IsPassed)
                .Select(ul => new UserLessonViewModel
                {
                    LessonId = ul.LessonId ?? 0,
                    PassedLevel = ul.PassedLevel,
                    Title = ul.Lesson?.Title ?? "Unknown",
                    RequiredLevel = ul.Lesson?.RequiredLevel ?? 0
                }).ToList();


            int userScore = userLessons.Sum(ul => ul.PassedLevel ?? 0);

            var last7Days = Enumerable.Range(0, 7)
            .Select(i => DateTime.UtcNow.AddDays(-i).Date)
            .ToList();
            List<UserExperience> weeklyExperience = _userExperiance.GetAll()
            .Where(ue => ue.Date >= last7Days.Min() && ue.UserId == user.Id)
            .GroupBy(ue => ue.Date.Date)
            .Select(g => new UserExperience
            {
                Date = g.Key,
                ExperiencePoints = g.Sum(ue => ue.ExperiencePoints)
            }).ToList();
            weeklyExperience = last7Days
            .Select(day => weeklyExperience.FirstOrDefault(we => we.Date == day) ?? new UserExperience { Date = day, ExperiencePoints = 0 })
            .OrderBy(we => we.Date).ToList();

            ViewBag.EditAccess = adminUser || isOwner;
            ViewBag.DeleteAccess = adminUser;
            ViewBag.IsAdmin = adminUser && isOwner;
            ViewBag.RoleIdent = await _userManager.IsInRoleAsync(user, "user");
            ViewBag.UserScore = userScore;

            var model = new UserProfileViewModel
            {
                User = user,
                UserLevels = userLevels,
                UserLessons = userLessons,
                WeeklyExperience = weeklyExperience
            };

            return View(model);
        }
        return NotFound();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);

        ViewBag.User = user;
        var viewModel = new EditViewModel()
        {
            BirthDate = DateOnly.FromDateTime(user.BirthDate.Value),
            District = user.Disctrict,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
        };
        return View(viewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(EditViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (model.UploadedFile is not null)
            {
                string path = "/userImages/defProf-ProfileN=1.png";
                if (user.AvatarUrl != path)
                {
                    var fullPath = _environment.WebRootPath + user.AvatarUrl;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                string newFileName = Path.ChangeExtension($"{user.UserName.Trim()}-ProfileN=1", Path.GetExtension(model.UploadedFile.FileName));
                path = $"/userImages/" + newFileName.Trim();
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await model.UploadedFile.CopyToAsync(fileStream);
                }
                user.AvatarUrl = path;
            }
            user.FullName = model.FullName;
            user.Disctrict = model.District;
            user.BirthDate = model.BirthDate.ToDateTime(TimeOnly.MinValue);
            user.PhoneNumber = model.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile");
        }
        return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, IFormFile? uploadedFile)
    {
        if (ModelState.IsValid)
        {
            string path = "/userImages/defProf-ProfileN=1.png";
            if (uploadedFile != null)
            {
                string newFileName = Path.ChangeExtension($"{model.UserName.Trim()}-ProfileN=1", Path.GetExtension(uploadedFile.FileName));
                path = $"/userImages/" + newFileName.Trim();
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
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
                Disctrict = model.District,
                AvatarUrl = path,
                BirthDate = model.BirthDate.ToUniversalTime(),
                CreatedAt = DateTime.UtcNow.AddHours(6),
                Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _userManager.SetLockoutEnabledAsync(user, false);

                var startTariff = _tariffs.GetAll().FirstOrDefault(t => t.Name == "Start");
                user.TariffId = startTariff.Id;
                user.TariffStartDate = DateTime.UtcNow;
                user.TariffEndDate = null;
                await _userManager.UpdateAsync(user);

                var (subject, message) = await GenerateEmailConfirmationContentAsync(user, model.UserName);
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(model.Email, subject, message);
                return Json(new { success = true, email = user.Email });
            }
        }

        ModelState.AddModelError("", _localizer["Wrong"]);
        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
    }

    private async Task<(string subject, string message)> GenerateEmailConfirmationContentAsync(User user, string userName)
    {
        var code = _twoFactorService.GenerateCode(user.Id);
        var subject = _localizer["YourCode"];
        string message = await _razorTemplateEngine.RenderAsync("~/Views/Account/EmailConfirmationMessage.cshtml",
            new EmailConfirmationViewModel()
            {
                Code = code,
                Username = user.UserName
            });
        return (subject, message);
    }


    [HttpPost]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmCodeRequest model)
    {
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Code))
        {
            ModelState.AddModelError("", _localizer["EnterCode"]);
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", _localizer["NotFound"]);
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var isCodeValid = _twoFactorService.VerifyCode(user.Id, model.Code);
        if (!isCodeValid)
        {
            ModelState.AddModelError("", _localizer["WrongCode"]);
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
        await _signInManager.SignInAsync(user, true);


        return Json(new { success = true, userId = user.Id });
    }

    [HttpPost]
    public async Task<IActionResult> ResendCode([FromBody] ResendCodeRequest model)
    {
        if (string.IsNullOrEmpty(model.Email))
        {
            return Json(new { success = false, errors = new[] { "Email is required." } });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Json(new { success = false, errors = new[] { "User not found." } });
        }
        var (subject, message) = await GenerateEmailConfirmationContentAsync(user, user.UserName);
        EmailSender emailSender = new EmailSender();
        emailSender.SendEmail(user.Email, subject, message);

        return Json(new { success = true });
    }


    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel() { ReturnUrl = returnUrl });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = await _userManager.FindByEmailAsync(model.LoginValue);
            if (user == null)
                user = await _userManager.FindByNameAsync(model.LoginValue);
            if (user == null)
                user = _userService.GetAll().FirstOrDefault(u => u.PhoneNumber.Equals(model.LoginValue));
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    return RedirectToAction("Profile", "Account", new { id = user.Id });
                }
            }
            ModelState.AddModelError("LoginValue", "Invalid email, login or password!");
        }
        ModelState.AddModelError("", "Invalid provided form!");
        return View(model);
    }

    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [Authorize]
    public async Task<IActionResult> Delete()
    {
        var user = await _userManager.GetUserAsync(User);
        string path = "/userImages/defProf-ProfileN=1.png";
        if (user.AvatarUrl != path)
        {
            var fullPath = _environment.WebRootPath + user.AvatarUrl;
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
        await _signInManager.SignOutAsync();
        await _userManager.DeleteAsync(user);

        return RedirectToAction("Register", "Account");
    }
    private int GetCurrentUserId()
    {
        var userId = int.Parse(_userManager.GetUserId(User));
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> LogTime([FromBody] long timeSpent)
    {
        var currentUser = await _userService.GetById(int.Parse(_userManager.GetUserId(User)));
        if (currentUser == null)
            return Unauthorized();

        TimeSpan timeSpentTimeSpan = TimeSpan.FromMilliseconds(timeSpent);

        currentUser.Statistic.StudyMinutes += timeSpentTimeSpan;

        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка при обновлении данных пользователя.");
        }

        return Ok(new { message = "Время успешно сохранено" });
    }

}