using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Models.Dto
{
    public class PersonalInformationDto
    {
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public required string PersonalIdentificationNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required byte[] ProfilePicture { get; set; }
        public AddressDto Address { get; set; }

    }
}
