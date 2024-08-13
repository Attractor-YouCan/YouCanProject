using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Repository;
using YouCan.Service.Service;
using YouCan.Services.Email;
using YouCan.ViewModels;

namespace YouCan.Mvc;
public class AccountController : Controller
{
    private readonly YouCanContext _context; 
    private IUserCRUD _userService;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private IWebHostEnvironment _environment;
    private ICRUDService<UserLevel> _userLevel;
    private ICRUDService<UserLessons> _userLessonService;
    private readonly TwoFactorService _twoFactorService;


    public AccountController(IUserCRUD userService, UserManager<User> userManager,
        SignInManager<User> signInManager, IWebHostEnvironment environment,
        TwoFactorService twoFactorService, ICRUDService<UserLevel> userLevel, ICRUDService<UserLessons> userLessonService)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _environment = environment;
        _twoFactorService = twoFactorService;
        _userLevel = userLevel;
        _userLessonService = userLessonService;
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

            ViewBag.EditAccess = adminUser || isOwner;
            ViewBag.DeleteAccess = adminUser;
            ViewBag.IsAdmin = adminUser && isOwner;
            ViewBag.RoleIdent = await _userManager.IsInRoleAsync(user, "user");
            ViewBag.UserScore = userScore;

            var model = new UserProfileViewModel
            {
                User = user,
                UserLevels = userLevels,
                UserLessons = userLessons
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
                CreatedAt = DateTime.UtcNow.AddHours(6)
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

                var (subject, message) = GenerateEmailConfirmationContentAsync(user, model.UserName);
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(model.Email, subject, message);
                return Json(new { success = true, email = user.Email });
            }
        }

        ModelState.AddModelError("", "Что-то пошло не так! Пожалуйста, проверьте всю информацию");
        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
    }

    private (string subject, string message) GenerateEmailConfirmationContentAsync(User user, string userName)
    {
        var code = _twoFactorService.GenerateCode(user.Id);
        var subject = "Ваш код подтверждения";
        string message = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                    color: #333;
                    background-color: #f4f4f4;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    border: 1px solid #ddd;
                    border-radius: 10px;
                    background-color: #fff;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    background-color: #1252b3;
                    color: white;
                    padding: 10px;
                    border-top-left-radius: 10px;
                    border-top-right-radius: 10px;
                }}
                .header h1 {{
                    margin: 0;
                }}
                .content {{
                    padding: 20px;
                }}
                .content p {{
                    margin: 10px 0;
                }}
                .content a {{
                    color: #4CAF50;
                    text-decoration: none;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin: 20px 0;
                    font-size: 16px;
                    color: white;
                    background-color: #1252b3;
                    border: none;
                    border-radius: 5px;
                    text-align: center;
                    text-decoration: none;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #777;
                    border-top: 1px solid #ddd;
                    background-color: #f7f7f7;
                    border-bottom-left-radius: 10px;
                    border-bottom-right-radius: 10px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Добро пожаловать в YouCan!</h1>
                </div>
                <div class='content'>
                    <p>Твой никнейм:</p>
                    <p><strong>{userName}</strong></p>
                    <p>Код активации:</p>
                    <h2>{code}</h2>
                </div>
                <div class='footer'>
                    <p>Благодарим что присоединилсь к YouCan!</p>
                </div>
            </div>
        </body>
        </html>";
        return (subject, message);
    }


    [HttpPost]
    public async Task<IActionResult> ConfirmCode([FromBody] ConfirmCodeRequest model)
    {
        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Code))
        {
            ModelState.AddModelError("", "Ведите код!.");
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Пользователь не найден!.");
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var isCodeValid = _twoFactorService.VerifyCode(user.Id, model.Code);
        if (!isCodeValid)
        {
            ModelState.AddModelError("", "Неправильный код!.");
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
        await _signInManager.SignInAsync(user, true);
        

        return Json(new { success = true, userId = user.Id  });
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
        var (subject, message) = GenerateEmailConfirmationContentAsync(user, user.UserName);
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
    
    [Authorize]
    public async Task<IActionResult> Rating()
    {
        return View();
    }

}