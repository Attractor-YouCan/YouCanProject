using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Controllers;

public class AccountController : Controller
{
    
    private YouCanContext _db;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private IWebHostEnvironment _environment;


    public AccountController(YouCanContext db, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
        _environment = environment;
    }
    
    
    [Authorize]
    public async Task<IActionResult> Profile(int? userId)
    {
        User user = new User();
        if (!userId.HasValue)
            user = await _db.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(_userManager.GetUserId(User)));
        else
            user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var adminUser =await _userManager.IsInRoleAsync(currentUser, "admin");
            var isOwner = currentUser.Id == user.Id;
            ViewBag.EditAccess = adminUser || isOwner;
            ViewBag.DeleteAccess = adminUser;
            ViewBag.IsAdmin = adminUser && isOwner;
            ViewBag.RoleIdent = await _userManager.IsInRoleAsync(user, "user");
            return View(user);
        }
        return NotFound();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(User model, int userId, string? currentPassword, string? password)
    {
        User? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            if (password != null && currentPassword != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return NotFound();
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            await _userManager.UpdateAsync(user);
            return new ObjectResult(user);
        }
        ModelState.AddModelError("","Пользователь не найден");
        return NotFound();
    }
    
    public async Task<IActionResult> Register()
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
                path= $"/userImages/" + newFileName.Trim();
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
                FullName = model.LastName+ " " + model.FirstName,
                AvatarUrl = path,
                BirthDate = model.BirthDate,
                CreatedAt = DateTime.UtcNow.AddHours(6)
                
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Profile", "Account", new {userId = user.Id});
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        ModelState.AddModelError("", "Something went wrong! Please check all info");
        return View(model);
    }

    public async Task<IActionResult> ConfirmEmail()
    {
        int code = 1234;
        return Ok();
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel(){ReturnUrl = returnUrl});
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
                user = await _db.Users.FirstOrDefaultAsync(u => u.PhoneNumber.Equals(model.LoginValue));
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
                    return RedirectToAction("Profile", "Account", new {id = user.Id});
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
}