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

        /*/
        bool Verify()
        {
            bool validPassword = BCrypt.Net.BCrypt.Verify("submittedPassword", "");
        }*/
    }
}