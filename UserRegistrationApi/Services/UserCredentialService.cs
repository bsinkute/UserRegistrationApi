using System.Security.Cryptography;
using System.Text;
using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Services
{
    public interface IUserCredentialService
    {
        HashedCredentials GetHashedCredentials(string password);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt);
    }

    public class UserCredentialService : IUserCredentialService
    {
        public HashedCredentials GetHashedCredentials(string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            return new HashedCredentials
            {
                Password = passwordHash,
                Salt = passwordSalt
            };
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
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
 