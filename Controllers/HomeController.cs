using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Data;
using Shellty_Blog.Models;

namespace Shellty_Blog.Controllers;

public class HomeController : Controller
{
    private readonly BlogContext _context;

    public HomeController(BlogContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _context.BlogPosts
            .OrderByDescending(p => p.CreatedDate)
            .Take(3)
            .ToListAsync();
        return View(posts);
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