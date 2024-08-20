using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Database;

namespace UserRegistrationApi.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
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
    }
}
