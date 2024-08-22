using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Models.Dto
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Role { get; set; }
        public PersonalInformationDto PersonalInformation { get; set; }
    }
}
