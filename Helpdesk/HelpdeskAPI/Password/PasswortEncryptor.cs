using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace HelpdeskAPI.Password
{
    public class PasswortEncryptor
    {

        public static byte[] _salt = Encoding.UTF8.GetBytes("f4561klhkj&))gu");



        public static string ComputeHash(string input)
        {

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: input!,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));


            return hashed;
        }
    }
}
