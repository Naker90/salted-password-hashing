using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Security
{
    public class BCryptPasswordEncryptionService : PasswordEncryptionService
    {
        string Encrypt(string password, Password.Salt salt)
        {
            var saltedPassword = password + salt.Value;
            return BCrypt.Net.BCrypt.HashPassword(saltedPassword);
        }

        /*/
        bool Verify()
        {
            bool validPassword = BCrypt.Net.BCrypt.Verify("submittedPassword", "");
        }*/
    }
}