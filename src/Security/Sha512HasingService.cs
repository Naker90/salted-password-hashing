using SaltedPasswordHashing.Src.Domain.Security;
using System.Security.Cryptography;
using System.Text;

namespace SaltedPasswordHashing.Src.Security
{
    public class Sha512HasingService : HashingService
    {
        public string Hash(string input)
        {
            var data = Encoding.UTF8.GetBytes(input); 
            using(SHA512 sha512 = new SHA512Managed())
            {
                return sha512.ComputeHash(data).ToString();
            }
        }

        public bool Verify(string hash, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}