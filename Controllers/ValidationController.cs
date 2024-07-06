using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YouCan.Models;

namespace YouCan.Controllers;

public class ValidationController : Controller
{
    private YouCanContext _db;
    private UserManager<User> _userManager;

    public ValidationController(YouCanContext context, UserManager<User> userManager)
    {
        _db = context;
        _userManager = userManager;
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckEmail(string Email)
    {
        return !_db.Users.Any(u => u.Email.ToLower().Trim() == Email.ToLower().Trim());
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
        return !_db.Users.Any(u => u.PhoneNumber.ToLower().Trim() == PhoneNumber.ToLower().Trim());
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckUsername(string UserName)
    {
        return !_db.Users.Any(u => u.UserName.ToLower().Trim() == UserName.ToLower().Trim());
    }
    [HttpGet]
    public async Task<IActionResult> EditCheckUsernameEmail(string value, int? id)
    {
        User? adminUsr = await _userManager.GetUserAsync(User);
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        bool result = true;
        bool userName = value.ToLower().Trim() == adminUsr.UserName.ToLower().Trim() 
                        || value.ToLower().Trim() == user.UserName.ToLower().Trim();
        bool email = value.ToLower().Trim() == adminUsr.Email.ToLower().Trim() 
                     || value.ToLower().Trim() == user.Email.ToLower().Trim();
        if (_db.Users.Any(u => u.UserName.ToLower().Trim() == value.ToLower().Trim()) 
            || _db.Users.Any(u => u.Email.ToLower().Trim() == value.ToLower().Trim()))
        {
            if (userName || email )
                result = true;
            else
                result = false;
        }
        return Ok(result);
    }
}