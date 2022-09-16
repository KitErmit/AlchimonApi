using System;
using System.Security.Cryptography;
using System.Text;

namespace AlchimonAng.Helpers
{
    public class HashHepler
    {
        public string StringToSha256(string encodedLine)
        {
            SHA256 sha256 = SHA256Managed.Create();
            UTF8Encoding objUtf8 = new UTF8Encoding();
            var hashValue = sha256.ComputeHash(objUtf8.GetBytes(encodedLine));
            return Convert.ToBase64String(hashValue);
        }
    }
}

