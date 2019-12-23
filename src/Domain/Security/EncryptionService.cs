using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.Security
{
    public interface EncryptionService
    {
        string Encrypt(string input);
        bool Verify(string hash, string text);
    }
}