using System.Net;
using EchoBlog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EchoBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            //call respository to upload image and get URL
            var imageUrl = await _imageRepository.UploadImageAsync(file);

            if(imageUrl == null)
            {
                return Problem("Something went wrong while uploading the image!", null, (int)HttpStatusCode.InternalServerError);
            }
            return new JsonResult(new { link = imageUrl });
        }
    }
}