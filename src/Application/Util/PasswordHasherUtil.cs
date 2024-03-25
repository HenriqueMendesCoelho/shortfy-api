using System.Security.Cryptography;
using System.Text;

namespace suavesabor_api.src.Application.Util
{
    public class PasswordHasherUtil
    {
        public static string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();

            return CreateHash(password, salt);
        }

        public static bool VerifyPassword(string password, string storedPasswordWithSalt)
        {
            string[] parts = storedPasswordWithSalt.Split(':');
            string storedPassword = parts[0];
            string storedSalt = parts[1];
            if (parts.Length != 2)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(storedSalt);
            string hashedPasswordWithSalt = CreateHash(password, salt);
            string hashedPassword = hashedPasswordWithSalt.Split(':')[0];

            return hashedPassword == storedPassword;
        }

        private static string CreateHash(string password, byte[] salt)
        {
            using var hmac = new HMACSHA384(salt);
            byte[] hashedBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            string hashedPassword = builder.ToString();
            string storedPassword = hashedPassword + ":" + Convert.ToBase64String(salt);

            return storedPassword;
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            return salt;
        }
    }
}
