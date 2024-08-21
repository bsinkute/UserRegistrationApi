namespace UserRegistrationApi.Domain.Models
{
    public class Address
    {
        public required Guid AddressId { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public required string ApartmentNumber { get; set; }
        public Guid PersonalInformationId { get; set; }
        public PersonalInformation PersonalInformation { get; set; }
    }
}
