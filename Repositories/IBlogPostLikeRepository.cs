using EchoBlog.Models.Domain;

namespace EchoBlog.Repositories
{
    public interface IBlogPostLikeRepository
    {
        Task<int> GetTotatlLikes(Guid blogPostId);

        Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);
         Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike);
    }
}