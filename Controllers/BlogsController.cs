using System.Security.Cryptography.X509Certificates;
using EchoBlog.Models.Domain;
using EchoBlog.Models.ViewModels;
using EchoBlog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Domain;

namespace EchoBlog.Controllers
{
    public class BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository) : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository = blogPostRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository = blogPostLikeRepository;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository = blogPostCommentRepository;

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var liked = false;
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
            var blogDetailsViewModel = new BlogDetailsViewModel();
            
            if (blogPost != null)
            {
                var totalLikes = await _blogPostLikeRepository.GetTotatlLikes(blogPost.Id);

                if (_signInManager.IsSignedIn(User))
                {
                    //get like fot the current user for the current blog post
                    var likesForBlog = await _blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = _userManager.GetUserId(User);

                    if(userId != null)
                    {
                        var likeFromUser = likesForBlog.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }
                
                //Get comments for the current blog post
                var blogComment = await _blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);
                var blogCommentsForView = new List<BlogComment>();

                foreach (var comment in blogComment)
                {
                    blogCommentsForView.Add(new BlogComment
                    {
                        Description = comment.Description,
                        DateAdded = comment.DateAdded,
                        UserName = (await _userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    });
                }

                blogDetailsViewModel = new BlogDetailsViewModel
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = blogCommentsForView
                };
            }

            return View(blogDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = model.Id,
                    Description = model.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await _blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = model.UrlHandle });
            }
            return View();
        }
    }
}