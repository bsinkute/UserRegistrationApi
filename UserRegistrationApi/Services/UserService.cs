using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Repositories;
using UserRegistrationApi.Models.Dto;

namespace UserRegistrationApi.Services
{
    public interface IUserService
    {
        Task RegisterAsync(CreateUserDto userDto);
        Task<User?> LoginAsync(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreateUserMapper _createUserMapper;
        private readonly IUserCredentialService _userCredentialService;

        public UserService(IUserRepository userRepository, ICreateUserMapper createUserMapper, IUserCredentialService userCredentialService)
        {
            _userRepository = userRepository;
            _createUserMapper = createUserMapper;
            _userCredentialService = userCredentialService;
        }

        public async Task RegisterAsync(CreateUserDto userDto)
        {
            var user = _createUserMapper.Bind(userDto);
            
            await _userRepository.AddUserAsync(user);
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserAsync(username);

            if (user == null)
            {
                return null;
            }

            if (_userCredentialService.VerifyPasswordHash(password, user.Password, user.Salt))
            {
                return user;
            }

            return null;
        }
    }
}
