using System.Security.Cryptography;
using System.Text;

namespace HRSystem.Application.Common
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            using var hmac = new HMACSHA512();


            passwordSalt = Convert.ToBase64String(hmac.Key);


            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordHash = Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrWhiteSpace(storedHash) || string.IsNullOrWhiteSpace(storedSalt))
                return false;

            using var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt));

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var computedHashBase64 = Convert.ToBase64String(computedHash);

            return computedHashBase64 == storedHash;
        }
    }
}
