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

            return new User
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username,
                Password = hashedCredentials.Password,
                Salt = hashedCredentials.Salt,
                Role = "User",
                PersonalInformation = new PersonalInformation
                {
                    PersonalInformationId = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    Surname = dto.Surname,
                    PersonalIdentificationNumber = dto.PersonalIdentificationNumber,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    ProfilePicture = _profilePictureService.GenerateProfilePicture(dto.Image),
                    Address = new Address
                    {
                        AddressId = Guid.NewGuid(),
                        City = dto.City,
                        Street = dto.Street,
                        HouseNumber = dto.HouseNumber,
                        ApartmentNumber = dto.ApartmentNumber,
                    }
                }
            };
        }
    }  
}
