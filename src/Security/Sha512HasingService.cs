using SaltedPasswordHashing.Src.Domain.Security;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace SaltedPasswordHashing.Src.Security
{
    public class Sha512HasingService : HashingService
    {
        public string Hash(string input)
        {
            var data = Encoding.UTF8.GetBytes(input); 
            using(SHA512 sha512 = new SHA512Managed())
            {
                var hash = sha512.ComputeHash(data);
                return GetStringFromHash(hash);
            }
        }

        private string GetStringFromHash(byte[] hash)
        {
            return string.Join("", hash.Select(b => b.ToString("x2")));
        }
    }
}