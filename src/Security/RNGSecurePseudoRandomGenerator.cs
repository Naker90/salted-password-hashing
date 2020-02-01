using System.Security.Cryptography;
using SaltedPasswordHashing.Src.Domain.Security;
using SaltedPasswordHashing.Src.Domain.Types;
using System;

namespace SaltedPasswordHashing.Src.Security
{
    public class RNGSecurePseudoRandomGenerator : SecurePseudoRandomGenerator
    {
        public Password.Salt Generate()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[64]; 
                rng.GetBytes(data);
                var value = BitConverter.ToString(data, 0).Replace("-", "");
                return new Password.Salt(value: value);
            }
        }   
    }
}