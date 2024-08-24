using YouCan.Mvc;

namespace YouCan.Tests.Services;

public class FakeTwoFactorService : TwoFactorService
{
    private readonly Dictionary<int, string> _userCodes = new Dictionary<int, string>();
    private readonly bool _verifyCodeResult;

    public FakeTwoFactorService(bool verifyCodeResult)
    {
        _verifyCodeResult = verifyCodeResult;
    }

    public string GenerateCode(int userId)
    {
        var code = new Random().Next(100000, 999999).ToString();
        _userCodes[userId] = code;
        return code;
    }

    public bool VerifyCode(int userId, string code)
    {
        if (_userCodes.TryGetValue(userId, out var storedCode))
        {
            return storedCode == code;
        }
        return _verifyCodeResult;
    }
}