using Microsoft.AspNetCore.Identity;


public class EmailTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : class
{
    public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(true);
    }

    public Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
    {
        var token = new Random().Next(100000, 999999).ToString();
        return Task.FromResult(token);
    }

    public Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
    {
        return Task.FromResult(true);
    }
}