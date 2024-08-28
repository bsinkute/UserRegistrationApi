using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Repositories;

namespace UserRegistrationApi.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid userId);
        Task<User> GetUserAsync(string username);
        Task<IEnumerable<User>> GetUsersAsync();
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
        Task<bool> DeleteUserAsync(Guid userId);

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user;
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userRepository.GetUserAsync(username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
           
        }

        private async Task<User> UpdatePersonalInformationPropertyAsync(Guid userId, Action<PersonalInformation> updateAction)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            updateAction(existingUser.PersonalInformation);
            await _userRepository.UpdateUserPersonalInformation(existingUser);

            return existingUser;
        }

        private async Task<User> UpdateAddressPropertyAsync(Guid userId, Action<Address> updateAction)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return null;
            }

            updateAction(existingUser.PersonalInformation.Address);
            await _userRepository.UpdateUserAddress(existingUser);

            return existingUser;
        }

        public Task<User> UpdateUserFirstNameAsync(Guid userId, string firstName)
        {
            return UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.FirstName = firstName);
        }

        public Task<User> UpdateUserSurnameAsync(Guid userId, string surname)
        {
            return UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.Surname = surname);
        }

        public Task<User> UpdateUserPersonalIdentificationNumberAsync(Guid userId, string personalIdentificationNumber)
        {
            return UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.PersonalIdentificationNumber = personalIdentificationNumber);
        }

        public Task<User> UpdateUserPhoneNumberAsync(Guid userId, string phoneNumber)
        {
            return UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.PhoneNumber = phoneNumber);
        }

        public Task<User> UpdateUserEmailAsync(Guid userId, string email)
        {
            return UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.Email = email);
        }

        public Task<User> UpdateUserProfilePictureAsync(Guid userId, byte[]profilePicture)
        {
            return UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.ProfilePicture = profilePicture);
        }

        public Task<User> UpdateUserCityAsync(Guid userId, string city)
        {
            return UpdateAddressPropertyAsync(userId, address => address.City = city);
        }

        public Task<User> UpdateUserStreetAsync(Guid userId, string street)
        {
            return UpdateAddressPropertyAsync(userId, address => address.Street = street);
        }

        public Task<User> UpdateUserHouseNumberAsync(Guid userId, string houseNumber)
        {
            return UpdateAddressPropertyAsync(userId, address => address.HouseNumber = houseNumber);
        }

        public Task<User> UpdateUserApartmentNumberAsync(Guid userId, string apartmentNumber)
        {
            return UpdateAddressPropertyAsync(userId, address => address.ApartmentNumber = apartmentNumber);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            await _userRepository.DeleteUserAsync(userId);
            return true;
        }
    }
}
