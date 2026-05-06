namespace EchoBlog.Repositories
{
    public interface IImageRepository
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}