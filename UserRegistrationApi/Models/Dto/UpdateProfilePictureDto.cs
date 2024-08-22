using UserRegistrationApi.Attributes;

namespace UserRegistrationApi.Models.Dto
{
    public class UpdateProfilePictureDto
    {
        [AllowedExtension([".jpg", ".png"])]
        public required IFormFile Image { get; set; }
    }
}
