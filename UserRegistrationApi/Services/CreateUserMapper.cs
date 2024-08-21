using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;

namespace UserRegistrationApi.Services
{
    public interface ICreateUserMapper
    {
        public User Bind(CreateUserDto user);
    }
    public class CreateUserMapper : ICreateUserMapper
    {
        private readonly IUserCredentialService _userCredentialService;
        private readonly IProfilePictureService _profilePictureService;

        public CreateUserMapper(IUserCredentialService userCredentialService, IProfilePictureService profilePictureService)
        {
            _userCredentialService = userCredentialService;
            _profilePictureService = profilePictureService;
        }

        public User Bind(CreateUserDto dto)
        {
            var hashedCredentials = _userCredentialService.GetHashedCredentials(dto.Password);

            var userId = Guid.NewGuid();
            var personalInformationId = Guid.NewGuid();
            var addressId = Guid.NewGuid();

            var user = new User
            {
                UserId = userId,
                Username = dto.Username,
                Password = hashedCredentials.Password,
                Salt = hashedCredentials.Salt,
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = personalInformationId,
                    FirstName = dto.FirstName,
                    Surname = dto.Surname,
                    PersonalIdentificationNumber = dto.PersonalIdentificationNumber,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    ProfilePicture = _profilePictureService.GenerateProfilePicture(dto.Image),
                    UserId = userId,
                    Address = new Address
                    {
                        AddressId = addressId,
                        City = dto.City,
                        Street = dto.Street,
                        HouseNumber = dto.HouseNumber,
                        ApartmentNumber = dto.ApartmentNumber,
                        PersonalInformationId = personalInformationId,
                    },
                },
            };

            return user;
        }
    }  
}
