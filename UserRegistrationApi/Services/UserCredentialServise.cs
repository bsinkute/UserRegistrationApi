using System.Security.Cryptography;
using System.Text;

namespace UserRegistrationApi.Services
{
    public class UserCredentialServise
    {
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(passwordHash);

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
 