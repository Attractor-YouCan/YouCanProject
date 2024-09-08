using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Mvc.Controllers;

public class LanguageController : Controller
{
    private UserManager<User> _userManager;
    public LanguageController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IActionResult> SetLanguage(string culture, string returnUrl)
    {
        if (!string.IsNullOrEmpty(culture) && (culture == "ru" || culture == "ky"))
        {
            if(User?.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                user.Language = culture;
                await _userManager.UpdateAsync(user);
            }
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }
        if(Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        return RedirectToAction("Index", "Home");
    }
}
