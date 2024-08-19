namespace UserRegistrationApi.Domain.Models
{
    public class Address
    {
        public required Guid Id { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string HouseNumber { get; set; }
        public required string ApartmentNumber { get; set; }
    }
}
