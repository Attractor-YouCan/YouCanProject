using System.Collections.Concurrent;

namespace YouCan.Mvc
{
    public class TwoFactorService
    {
        private static readonly ConcurrentDictionary<int, string> UserCodes = new ConcurrentDictionary<int, string>();

        public string GenerateCode(int userId)
        {
            var code = new Random().Next(100000, 999999).ToString();
            UserCodes[userId] = code;
            return code;
        }


        public bool VerifyCode(int userId, string code)
        {
            if (UserCodes.TryGetValue(userId, out var storedCode))
            {
                if (storedCode == code)
                {
                    UserCodes.TryRemove(userId, out _);
                    return true;
                }
            }

            return false;
        }
    }
}