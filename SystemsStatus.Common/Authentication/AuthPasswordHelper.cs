using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SystemsStatus.Common.Authentication
{
    public static class AuthPasswordHelper
    {
        public static byte[] GenerateSalt(int size)
        {
            using (var crypto = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[size];
                crypto.GetBytes(bytes); //get a bucket of very random bytes
                return bytes;
            }
        }

        public static string HashEncode(string password, byte[] salt, int iterations)
        {
            var deriver = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = deriver.GetBytes(1024);
            return Convert.ToBase64String(hash);
        }
    }
}
