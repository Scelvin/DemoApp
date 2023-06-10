using System.Security.Cryptography;
using System.Text;


namespace DemoApp
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return ConvertToHexString(hashedBytes);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            string enteredPasswordHashed = HashPassword(enteredPassword);
            return enteredPasswordHashed.Equals(hashedPassword);
        }

        private static string ConvertToHexString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}