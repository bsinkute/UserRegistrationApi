using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;

namespace UserRegistrationApi.Services
{
    public interface IUserDtoMapper
    {
        public UserDto Bind(User user);
    }
    public class UserDtoMapper : IUserDtoMapper
    {
        public UserDto Bind(User user)
        {
            return new UserDto
            {
                Username = user.Username,
                Role = user.Role,
                PersonalInformation = new PersonalInformationDto
                {
                    FirstName = user.PersonalInformation.FirstName,
                    Surname = user.PersonalInformation.Surname,
                    PersonalIdentificationNumber = user.PersonalInformation.PersonalIdentificationNumber,
                    PhoneNumber = user.PersonalInformation.PhoneNumber,
                    Email = user.PersonalInformation.Email,
                    ProfilePicture = user.PersonalInformation.ProfilePicture,
                    Address = new AddressDto
                    {
                        City = user.PersonalInformation.Address.City,
                        Street = user.PersonalInformation.Address.Street,
                        HouseNumber = user.PersonalInformation.Address.HouseNumber,
                        ApartmentNumber = user.PersonalInformation.Address.ApartmentNumber,
                    },
                },
            };
        }
    }
}
