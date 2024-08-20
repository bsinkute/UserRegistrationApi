using UserRegistrationApi.Attributes;
using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Models.Dto
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public required string PersonalIdentificationNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        [AllowedExtension([".jpg", ".png"])]
        public required IFormFile Image { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public required string ApartmentNumber { get; set; }
    }
}
