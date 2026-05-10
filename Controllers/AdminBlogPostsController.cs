using EchoBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace EchoBlog.Controllers
{
    [Authorize(Roles = "Admin")]
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
                FeaturedImageUrl = request.FeaturedImageUrl,
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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await _blogPostRepository.GetAsync(id);
            var tags = await _tagRepository.GetAllAsync();

            if(blogPost != null)
            {
                var editBlogPostRequest = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Visible = blogPost.Visible,
                    Tags = tags.Select(x => new SelectListItem
                    {
                        Text = x.Name, Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(t => t.Id.ToString()).ToArray() //to pre-select the tags associated with the blog post
                };
                return View(editBlogPostRequest);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest request)
        {
            var blogPostDDomainModel = new BlogPost //mapping view model to domain model
            {
                Id = request.Id,
                Heading = request.Heading,
                PageTitle = request.PageTitle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author = request.Author,
                Visible = request.Visible
            };
            //map tags into domain model
            var selectedTags = new List<Tag>();
            foreach(var selectedTagId in request.SelectedTags)
            {
                if(Guid.TryParse(selectedTagId, out var tagId))
                {
                    var existingTag = await _tagRepository.GetAsync(tagId);
                    if(existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
            }
            blogPostDDomainModel.Tags = selectedTags; //mapping tags back to domain model
            var updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPostDDomainModel); //submiting the updated blog post to the repository for updating in the database
            if(updatedBlogPost != null)
            {
                //show success notification
                return RedirectToAction("Edit");
            }
            return RedirectToAction("Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest request)
        {
            var deletedBlogPost = await _blogPostRepository.DeleteAsync(request.Id);
            if(deletedBlogPost != null)
            {
                //show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }
    }
}