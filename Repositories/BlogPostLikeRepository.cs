using EchoBlog.Data;
using EchoBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace EchoBlog.Repositories
{
    public class BlogPostLikeRepository(BlogDbContext blogDbContext) : IBlogPostLikeRepository
    {
        private readonly BlogDbContext _blogDbContext = blogDbContext;

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await _blogDbContext.BlogPostLikes.AddAsync(blogPostLike);
            await _blogDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
        {
            return await _blogDbContext.BlogPostLikes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotatlLikes(Guid blogPostId)
        {
            return await _blogDbContext.BlogPostLikes.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}