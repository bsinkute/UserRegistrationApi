using Microsoft.EntityFrameworkCore;
using System.Net;
using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Repositories;
using UserRegistrationApi.Models.Dto;

namespace UserRegistrationApi.Services
{
    public interface IPersonalInformationService
    {
        Task<User> GetUserAsync(Guid userId);
        Task<User> UpdateUserFirstNameAsync(Guid userId, string firstName);
    }
    public class PersonalInformationService : IPersonalInformationService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICreateUserMapper _createUserMapper;
        private readonly IUserCredentialService _userCredentialService;

        public PersonalInformationService(IUserRepository userRepository, ICreateUserMapper createUserMapper, IUserCredentialService userCredentialService)
        {
            _userRepository = userRepository;
            _createUserMapper = createUserMapper;
            _userCredentialService = userCredentialService;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<User> UpdateUserFirstNameAsync(Guid userId, string firstName)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.FirstName = firstName;

            await _userRepository.UpdateUserPersonalInformation(existingUser);
            return existingUser;
        }
    }
}
