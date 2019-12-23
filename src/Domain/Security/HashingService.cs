using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.Security
{
    public interface HashingService
    {
        string Encrypt(string input);
        bool Verify(string hash, string text);
    }
}