namespace UserRegistrationApi.Domain.Models
{
    public class User
    {
        public required Guid UserId { get; set; }
        public required string Username { get; set; }
        public required byte[] Password { get; set; }
        public required byte[] Salt { get; set; }
        public required string Role { get; set; }
        public required Guid PersonalInformationId { get; set; }
        public required PersonalInformation PersonalInformation { get; set; }
        

    }
}
