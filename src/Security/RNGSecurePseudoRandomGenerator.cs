using System.Security.Cryptography.RNGCryptoServiceProvider;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Security
{
    public class RNGSecurePseudoRandomGenerator : SecurePseudoRandomGenerator
    {
        public Password.Salt Generate()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4]; 
                rng.GetBytes(data);
                return new Password.Salt(value: data.ToString());
            }
        }   
    }
}