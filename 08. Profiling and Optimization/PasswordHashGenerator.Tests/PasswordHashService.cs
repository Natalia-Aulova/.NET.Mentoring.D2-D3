using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordHashGenerator.Tests
{
    public class PasswordHashService
    {
        public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public string GeneratePasswordHashUsingSalt_v2(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public string GeneratePasswordHashUsingSalt_v3(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var passwordBytes = new UTF8Encoding(false).GetBytes(passwordText);
            var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes, 0, 36, Base64FormattingOptions.None);
            return passwordHash;
        }

        public string GeneratePasswordHashUsingSalt_v4(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var passwordBytes = new UTF8Encoding(false).GetBytes(passwordText);
             var pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, 20);

            var passwordHash = Convert.ToBase64String(hashBytes, 0, 36, Base64FormattingOptions.None);
            return passwordHash;
        }
    }
}
