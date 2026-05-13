using EchoBlog.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Models.Domain;

namespace EchoBlog.Data
{
    public class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options)
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLikes { get; set; }
    }
}