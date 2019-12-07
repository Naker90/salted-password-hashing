using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Domain.Security
{
    public interface SecurePseudoRandomGenerator
    {
         Password.Salt Generate();
    }
}