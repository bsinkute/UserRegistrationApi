using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

namespace UserRegistrationApi.Services
{
    public interface IProfilePictureService
    {
        byte[] GenerateProfilePicture(IFormFile picture);
    }

    public class ProfilePictureService : IProfilePictureService
    {
        public byte[] GenerateProfilePicture(IFormFile picture)
        {
            const int profilePictureWidth = 200;
            const int profilePicturHeight = 200;

            using (var ms = new MemoryStream())
            {
                picture.CopyTo(ms);
                ms.Position = 0;
                using (var originalPicture = SixLabors.ImageSharp.Image.Load(ms))
                {
                    int originalWidth = originalPicture.Width;
                    int originalHeight = originalPicture.Height;

                    float scale = Math.Min((float)profilePictureWidth / originalWidth, (float)profilePicturHeight / originalHeight);
                    int scaledWidth = (int)(originalWidth * scale);
                    int scaledHeight = (int)(originalHeight * scale);

                    originalPicture.Mutate(x => x.Resize(scaledWidth, scaledHeight));

                    using (var savedPictureStream = new MemoryStream())
                    {
                        originalPicture.Save(savedPictureStream, originalPicture.DetectEncoder(picture.FileName));
                        return savedPictureStream.ToArray();
                    }
                }
            }
        }
    }
}
