using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;
using YouCan.ViewModels.Account;

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
        if(ModelState.IsValid)
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
                Disctrict = model.District,
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
        
        return RedirectToAction("Login", "Account");
    }
}