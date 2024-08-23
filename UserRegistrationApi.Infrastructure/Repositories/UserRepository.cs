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
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task UpdateUserPersonalInformation(User user);
        Task UpdateUserAddress(User user);
        Task DeleteUserAsync(Guid id);
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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var user = await _context.Users
                .Include(x => x.PersonalInformation)
                .ThenInclude(x => x.Address)
                .ToListAsync();

            return user;
        }

        public async Task UpdateUserPersonalInformation(User user)
        {
            _context.PersonalInformation.Update(user.PersonalInformation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAddress(User user)
        {
            _context.Address.Update(user.PersonalInformation.Address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
