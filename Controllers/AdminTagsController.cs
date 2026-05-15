using EchoBlog.Data;
using EchoBlog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.ViewModels;
using System.Linq;

namespace EchoBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest request)
        {
            ValidateAddTagRequest(request);
            if (!ModelState.IsValid)
            {
                return View();
            }
            var newTag = new Tag
            {
                Name = request.Name,
                DisplayName = request.DisplayName
            };
            await _tagRepository.AddAsync(newTag);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection)
        {
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            var list = await _tagRepository.GetAllAsync(searchQuery, sortBy, sortDirection);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
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
            var updatedTag = await _tagRepository.UpdateAsync(Tag);
            if(updatedTag != null)
            {
                return RedirectToAction("List");
            }
            else
            {
                // Tag not found or update failed
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest request)
        {
            var deletedTag = await _tagRepository.DeleteAsync(request.Id);
            if(deletedTag != null)
            {
                // Tag deleted successfully
            }
            else
            {
                // Tag not found or deletion failed
            }
            return RedirectToAction("Edit", new { id = request.Id });
        }

        //custom validations
        private void ValidateAddTagRequest(AddTagRequest request)
        {
            if(request.Name != null && request.DisplayName != null)
            {
                if(request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Display name cannot be the same as Name");
                }
            }
        }
    }
}