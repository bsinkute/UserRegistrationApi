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
            using var hmac = new HMACSHA512();
            return new HashedCredentials
            {
                Password = hmac.Key,
                Salt = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            };
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(passwordHash);
        }
    }
}
 