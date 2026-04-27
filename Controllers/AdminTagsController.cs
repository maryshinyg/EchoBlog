using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.ViewModel;
using System.Linq;

namespace EchoBlog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BlogDbContext _blogDbContext;
        public AdminTagsController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.Name,
                DisplayName = request.DisplayName
            };
            await _blogDbContext.Tags.AddAsync(newTag);
            await _blogDbContext.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var list = await _blogDbContext.Tags.ToListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if(tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest request)
        {
            var Tag = new Tag
            {
                Id = request.Id,
                Name = request.Name,
                DisplayName = request.DisplayName
            };
            var existingTag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(existingTag != null)
            {
                existingTag.Name = Tag.Name;
                existingTag.DisplayName = Tag.DisplayName;
                await _blogDbContext.SaveChangesAsync();
                return RedirectToAction("List"); //"Edit", new { id = request.Id }
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest request)
        {
            var existingTag = await _blogDbContext.Tags.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(existingTag != null)
            {
                _blogDbContext.Tags.Remove(existingTag);
                await _blogDbContext.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }
    }
}