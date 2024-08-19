namespace UserRegistrationApi.Domain.Models
{
    public class PersonalInformation
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string Surname { get; set; }
        public required string PersonalIdentificationNumber { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email {  get; set; }
        public required byte[] ProfilePicture { get; set; }
        public required Guid AdressId { get; set; }
        public required Address Address { get; set; }

    }
}
