using Microsoft.EntityFrameworkCore;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Database;

namespace UserRegistrationApi.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserAsync(string username);
        Task<User> GetUserByIdAsync(Guid userId);
        Task UpdateUserPersonalInformation(User user);
        Task UpdateUseAddress(User user);
    }
    public class UserReposotory : IUserRepository
    {
        private readonly UserRegistrationDbContext _context;

        public UserReposotory(UserRegistrationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            return user;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(x => x.PersonalInformation)
                .ThenInclude(x => x.Address)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            return user;
        }

        public async Task UpdateUserPersonalInformation(User user)
        {
            _context.PersonalInformation.Update(user.PersonalInformation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUseAddress(User user)
        {
            _context.Address.Update(user.PersonalInformation.Address);
            await _context.SaveChangesAsync();
        }
    }
}
