using UserRegistrationApi.Domain.Models;

namespace UserRegistrationApi.Models.Dto
{
    public class AddressDto
    {
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public required string ApartmentNumber { get; set; }
    }
}
