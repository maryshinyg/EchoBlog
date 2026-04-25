using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Domain;
using Models.ViewModel;

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
        public IActionResult Add(AddTagRequest request)
        {
            var newTag = new Tag
            {
                Name = request.Name,
                DisplayName = request.DisplayName
            };
            _blogDbContext.Tags.Add(newTag);
            _blogDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List()
        {
            var list = _blogDbContext.Tags.ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = _blogDbContext.Tags.FirstOrDefault(x => x.Id == id);
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
        public IActionResult Edit(EditTagRequest request)
        {
            var Tag = new Tag
            {
                Id = request.Id,
                Name = request.Name,
                DisplayName = request.DisplayName
            };
            var existingTag = _blogDbContext.Tags.FirstOrDefault(x => x.Id == request.Id);
            if(existingTag != null)
            {
                existingTag.Name = Tag.Name;
                existingTag.DisplayName = Tag.DisplayName;
                _blogDbContext.SaveChanges();
                return RedirectToAction("Edit", new { id = request.Id });
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }

        [HttpPost]
        public IActionResult Delete(EditTagRequest request)
        {
            var existingTag = _blogDbContext.Tags.FirstOrDefault(x => x.Id == request.Id);
            if(existingTag != null)
            {
                _blogDbContext.Tags.Remove(existingTag);
                _blogDbContext.SaveChanges();
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }
    }
}