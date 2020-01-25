using SaltedPasswordHashing.Src.Domain.Security;

namespace SaltedPasswordHashing.Src.Security
{
    public class BCryptHashingService : HashingService
    {
        public string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        public bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(hash, text);
        }
    }
}