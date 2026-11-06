using System;
using System.Security.Cryptography;

namespace QuanLyChamCong
{
    public class PasswordHelper
    {
        public static string BamMatKhau(string matKhau)
        {
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                matKhau,
                salt,
                100000,
                HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);

                return Convert.ToBase64String(salt)
                    + ":" + Convert.ToBase64String(hash);
            }
        }

        public static bool KiemTraMatKhau(
            string matKhau,
            string matKhauDaBam)
        {
            string[] parts = matKhauDaBam.Split(':');

            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hashCu = Convert.FromBase64String(parts[1]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                matKhau,
                salt,
                100000,
                HashAlgorithmName.SHA256))
            {
                byte[] hashMoi = pbkdf2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(
                    hashCu, hashMoi);
            }
        }
    }
}