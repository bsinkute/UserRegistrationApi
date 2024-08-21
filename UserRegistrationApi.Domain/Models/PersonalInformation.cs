namespace UserRegistrationApi.Domain.Models
{
    public class PersonalInformation
    {
        public required Guid PersonalInformationId { get; set; }
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public required string PersonalIdentificationNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email {  get; set; }
        public required byte[] ProfilePicture { get; set; }
        public Guid AdressId { get; set; }
        public Address Address { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

    }
}
