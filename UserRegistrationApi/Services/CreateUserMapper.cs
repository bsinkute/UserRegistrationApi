using UserRegistrationApi.Domain.Models;
using UserRegistrationApi.Models.Dto;

namespace UserRegistrationApi.Services
{
    public interface ICreateUserMapper
    {
        public CreateUserDto Bind(User user);
        public User Bind(CreateUserDto user);
    }
    public class CreateUserMapper : ICreateUserMapper
    {
        public CreateUserDto Bind(User user)
        {
            return new CreateUserDto(user);
        }

        public User Bind(CreateUserDto user)
        {
            return new User
            {
                Username = user.Username,
                Password = user.Password,
                PersonalInformation = new PersonalInformation
                {
                    FirstName = user.FirstName,
                    Surname = user.Surname,
                    PersonalIdentificationNumber = user.PersonalIdentificationNumber,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    ProfilePicture = user.ProfilePicture,
                    Address = new Address
                    {
                        City = user.City,
                        Street = user.Street,
                        HouseNumber = user.HouseNumber,
                        ApartmentNumber = user.ApartmentNumber
                    }

                }
            };
        }
    }  
}
