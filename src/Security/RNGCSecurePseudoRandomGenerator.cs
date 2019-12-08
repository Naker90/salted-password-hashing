using System.Security.Cryptography.RNGCryptoServiceProvider;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.Types;

namespace SaltedPasswordHashing.Src.Security
{
    public class RNGCSecurePseudoRandomGenerator : SecurePseudoRandomGenerator
    {
        public Password.Salt Generate()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[4]; 
                rng.GetBytes(data);
                int salt = BitConverter.ToInt32(data, 0);
                return new Password.Salt(value: salt);
            }
        }   
    }
}