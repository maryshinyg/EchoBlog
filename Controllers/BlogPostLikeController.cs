using EchoBlog.Models.Domain;
using EchoBlog.Models.ViewModels;
using EchoBlog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EchoBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            _blogPostLikeRepository = blogPostLikeRepository;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest request)
        {
            var model = new BlogPostLike
            {
                BlogPostId = request.BlogPostId,
                UserId = request.UserId
            };
            await _blogPostLikeRepository.AddLikeForBlog(model);

            return Ok();
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/TotalLikes")]
        public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
        {
            var likes = await _blogPostLikeRepository.GetTotatlLikes(blogPostId);
            return Ok(likes);
        }
    }
}