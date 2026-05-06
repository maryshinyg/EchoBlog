using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace EchoBlog.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly Account _cloudinaryAccount;
        public CloudinaryImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _cloudinaryAccount = new Account(
                _configuration["Cloudinary:CloudName"],
                _configuration["Cloudinary:ApiKey"],
                _configuration["Cloudinary:ApiSecret"]
            );
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var client = new Cloudinary(_cloudinaryAccount);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };
            var uploadResult = await client.UploadAsync(uploadParams);

            if (uploadResult != null &&uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}