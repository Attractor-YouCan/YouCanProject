using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Controllers;

public class ValidationController : Controller
{
    private IUserCRUD _userService;
    private UserManager<User> _userManager;

    public ValidationController(IUserCRUD userService, UserManager<User> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckEmail(string Email)
    {
        return !_userService.GetAll().Any(u => u.Email.ToLower().Trim() == Email.ToLower().Trim());
    }

    [AcceptVerbs("GET", "POST")]
    public bool CheckBirthDate(DateTime BirthDate)
    {
        if (BirthDate > DateTime.UtcNow)
            return false;
        else if (BirthDate < DateTime.UtcNow.AddYears(-100))
            return false;
        else
            return true;
    }

    [HttpGet]
    public async Task<IActionResult> EditCheckPhoneNumber(string value)
    {
        User? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser!= null)
            if (currentUser.PhoneNumber == value)
                return Ok(true);
        return Ok(CheckPhoneNumber(value));
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckPhoneNumber(string PhoneNumber)
    {
        var b = !_userService.GetAll().Any(u => u.PhoneNumber.ToLower().Trim() == PhoneNumber.ToLower().Trim());
        return b;
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckUsername(string UserName)
    {
        return !_userService.GetAll().Any(u => u.UserName.ToLower().Trim() == UserName.ToLower().Trim());
    }
    [HttpGet]
    public async Task<IActionResult> EditCheckUsernameEmail(string value, int? id)
    {
        User? adminUsr = await _userManager.GetUserAsync(User);
        User? user = await _userService.GetById((int)id);
        bool result = true;
        bool userName = value.ToLower().Trim() == adminUsr.UserName.ToLower().Trim() 
                        || value.ToLower().Trim() == user.UserName.ToLower().Trim();
        bool email = value.ToLower().Trim() == adminUsr.Email.ToLower().Trim() 
                     || value.ToLower().Trim() == user.Email.ToLower().Trim();
        if (_userService.GetAll().Any(u => u.UserName.ToLower().Trim() == value.ToLower().Trim()) 
            || _userService.GetAll().Any(u => u.Email.ToLower().Trim() == value.ToLower().Trim()))
        {
            if (userName || email )
                result = true;
            else
                result = false;
        }
        return Ok(result);
    }
}