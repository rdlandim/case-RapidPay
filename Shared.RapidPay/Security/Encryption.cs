using System.Security.Cryptography;
using System.Text;

namespace Shared.RapidPay.Security
{
    public static class Encryption
    {
        public static string ToSha256(this string content)
        {
            var secret = Encoding.UTF8.GetBytes(content);

            return Convert.ToHexString(SHA256.HashData(secret));
        }
    }
}
