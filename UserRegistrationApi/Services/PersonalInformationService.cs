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
        Task<User> UpdateUserSurnameAsync(Guid userId, string surname);
        Task<User> UpdateUserPersonalIdentificationNumberAsync(Guid userId, string personalIdentificationNumber);
        Task<User> UpdateUserPhoneNumberAsync(Guid userId, string phoneNumber);
        Task<User> UpdateUserEmailAsync(Guid userId, string email);
        Task<User> UpdateUserProfilePictureAsync(Guid userId, byte[] profilePicture);
        Task<User> UpdateUserCityAsync(Guid userId, string city);
        Task<User> UpdateUserStreetAsync(Guid userId, string street);
        Task<User> UpdateUserHouseNumberAsync(Guid userId, string houseNumber);
        Task<User> UpdateUserApartmentNumberAsync(Guid userId, string apartmentNumber);

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

        public async Task<User> UpdateUserSurnameAsync(Guid userId, string surname)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.Surname = surname;

            await _userRepository.UpdateUserPersonalInformation(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserPersonalIdentificationNumberAsync(Guid userId, string personalIdentificationNumber)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.PersonalIdentificationNumber = personalIdentificationNumber;

            await _userRepository.UpdateUserPersonalInformation(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserPhoneNumberAsync(Guid userId, string phoneNumber)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.PhoneNumber = phoneNumber;

            await _userRepository.UpdateUserPersonalInformation(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserEmailAsync(Guid userId, string email)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.Email = email;

            await _userRepository.UpdateUserPersonalInformation(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserProfilePictureAsync(Guid userId, byte[]profilePicture)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.ProfilePicture = profilePicture;

            await _userRepository.UpdateUserPersonalInformation(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserCityAsync(Guid userId, string city)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.Address.City = city;

            await _userRepository.UpdateUserAddress(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserStreetAsync(Guid userId, string street)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.Address.Street = street;

            await _userRepository.UpdateUserAddress(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserHouseNumberAsync(Guid userId, string houseNumber)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.Address.HouseNumber = houseNumber;

            await _userRepository.UpdateUserAddress(existingUser);
            return existingUser;
        }

        public async Task<User> UpdateUserApartmentNumberAsync(Guid userId, string apartmentNumber)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.PersonalInformation.Address.ApartmentNumber = apartmentNumber;

            await _userRepository.UpdateUserAddress(existingUser);
            return existingUser;
        }
    }
}
