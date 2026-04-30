using EchoBlog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EchoBlog.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public AdminBlogPostsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
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

        public async Task<IActionResult> Add(AddBlogPostRequest request)
        {
            return RedirectToAction("Add");
        }
    }
}