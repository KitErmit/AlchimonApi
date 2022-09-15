using System;
using System.Security.Cryptography;
using System.Text;

namespace AlchimonAng.Services
{
    public class HashService
    {
        public HashService()
        {
        }

        public string StringToHash(string encodedLine)
        {
            SHA256 sha256 = SHA256Managed.Create();
            UTF8Encoding objUtf8 = new UTF8Encoding();
            var hashValue = sha256.ComputeHash(objUtf8.GetBytes(encodedLine));
            return Convert.ToBase64String(hashValue);
        }
    }
}

