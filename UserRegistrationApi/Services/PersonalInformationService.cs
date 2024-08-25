using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Infrastructure.Repositories;

namespace UserRegistrationApi.Services
{
    public interface IPersonalInformationService
    {
        Task<User> GetUserAsync(Guid userId);
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
    public class PersonalInformationService : IPersonalInformationService
    {
        private readonly IUserRepository _userRepository;

        public PersonalInformationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user;
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

        public async Task<User> UpdateUserFirstNameAsync(Guid userId, string firstName)
        {
            return await UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.FirstName = firstName);
        }

        public async Task<User> UpdateUserSurnameAsync(Guid userId, string surname)
        {
            return await UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.Surname = surname);
        }

        public async Task<User> UpdateUserPersonalIdentificationNumberAsync(Guid userId, string personalIdentificationNumber)
        {
            return await UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.PersonalIdentificationNumber = personalIdentificationNumber);
        }

        public async Task<User> UpdateUserPhoneNumberAsync(Guid userId, string phoneNumber)
        {
            return await UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.PhoneNumber = phoneNumber);
        }

        public async Task<User> UpdateUserEmailAsync(Guid userId, string email)
        {
            return await UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.Email = email);
        }

        public async Task<User> UpdateUserProfilePictureAsync(Guid userId, byte[]profilePicture)
        {
            return await UpdatePersonalInformationPropertyAsync(userId, personalInformation => personalInformation.ProfilePicture = profilePicture);
        }

        public async Task<User> UpdateUserCityAsync(Guid userId, string city)
        {
            return await UpdateAddressPropertyAsync(userId, address => address.City = city);
        }

        public async Task<User> UpdateUserStreetAsync(Guid userId, string street)
        {
            return await UpdateAddressPropertyAsync(userId, address => address.Street = street);
        }

        public async Task<User> UpdateUserHouseNumberAsync(Guid userId, string houseNumber)
        {
            return await UpdateAddressPropertyAsync(userId, address => address.HouseNumber = houseNumber);
        }

        public async Task<User> UpdateUserApartmentNumberAsync(Guid userId, string apartmentNumber)
        {
            return await UpdateAddressPropertyAsync(userId, address => address.ApartmentNumber = apartmentNumber);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            await _userRepository.DeleteUserAsync(userId);
            return true;
        }
    }
}
