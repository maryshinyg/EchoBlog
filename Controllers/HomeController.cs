using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EchoBlog.Models;
using EchoBlog.Repositories;
using Models.ViewModels;

namespace EchoBlog.Controllers;

public class HomeController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly ITagRepository _tagRepository;

    public HomeController(IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
    {
        _blogPostRepository = blogPostRepository;
        _tagRepository = tagRepository;
    }
    public async Task<IActionResult> Index()
    {
        var blogPosts = await _blogPostRepository.GetAllAsync();
        var tags = await _tagRepository.GetAllAsync();
        var model = new HomeViewModel
        {
            BlogPosts = blogPosts,
            Tags = tags
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
