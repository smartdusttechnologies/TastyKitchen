using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Neubel.Wow.Win.Authentication.Core.Model;

namespace Neubel.Wow.Win.Authentication.Common
{
    public class Hasher
    {
        #region Public methods
        public static PasswordLogin HashPassword(string password)
        {
            var salt = CreateSalt();
            var valueBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return new PasswordLogin { PasswordHash = Convert.ToBase64String(valueBytes), PasswordSalt = salt };
        }
        public static bool ValidateHash(string value, string salt, string hash, out string valueHash)
        {
            valueHash = CreateHash(value, salt);
            return valueHash == hash;
        }

        #endregion

        #region Private methods
        private static string CreateHash(string value, string salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        private static string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        #endregion
    }
}
