using System.Security.Cryptography;
using System.Text;

namespace cinema
{
    public static class StringExtensions
    {
        public static string GetSHA256Hash(this string str)
        {
            var hashBuilder = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                foreach (var b in result)
                    hashBuilder.Append(b.ToString("x2"));
            }

            return hashBuilder.ToString();
        }
    }
}