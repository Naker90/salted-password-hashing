using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Security
{
    public class BCryptPasswordEncryptionService : PasswordEncryptionService
    {
        public string Encrypt(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string hashedPassword, string passwordIntent)
        {
            return BCrypt.Net.BCrypt.Verify(passwordIntent, hashedPassword);
        }
    }
}