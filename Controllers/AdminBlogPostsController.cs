using EchoBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Domain;

namespace EchoBlog.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.DisplayName,
                    Value = x.Id.ToString()
                })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest request)
        {
            var blogPost = new BlogPost //mapping view model to domain model
            {
                Heading = request.Heading,
                PageTitle = request.PageTitle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturesImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                Visible = request.Visible
            };
            //map tags from selected tags 
            var selectedTags = new List<Tag>();
            foreach(var selectedTagId in request.SelectedTags)
            {
                var existingTag = await _tagRepository.GetAsync(Guid.Parse(selectedTagId));
                if(existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            } 
            blogPost.Tags = selectedTags; //mapping tags back to domain model
            await _blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var list = await _blogPostRepository.GetAllAsync();
            return View(list);
        }
    }
}