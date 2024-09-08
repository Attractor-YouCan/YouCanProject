using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using YouCan.Entities;

namespace YouCan.Mvc.Services;

public class UserProfileRequestCultureProvider : IRequestCultureProvider
{
    private UserManager<User> _userManager;
    public UserProfileRequestCultureProvider(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext.User?.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(httpContext.User);
            return new ProviderCultureResult(user.Language);
        }
        return null;
    }
}
