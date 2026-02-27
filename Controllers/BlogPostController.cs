using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Data;
using Shellty_Blog.Models;

namespace Shellty_Blog.Controllers;

public class BlogPostController : Controller
{
    private readonly BlogContext _context;
    private readonly IWebHostEnvironment _environment;

    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxImageSize = 2 * 1024 * 1024;

    public BlogPostController(BlogContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    private async Task LoadCategories()
    {
        ViewBag.Categories = await _context.BlogPosts
            .Where(p => !string.IsNullOrEmpty(p.Category))
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    private async Task<(string? FileName, string? Error)> SaveImageAsync(IFormFile file)
    {
        if (file.Length > MaxImageSize)
            return (null, "Image size must not exceed 2 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            return (null, "Only JPG, PNG, WebP, and GIF images are allowed.");

        var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads", "posts");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return (fileName, null);
    }

    private void DeleteImage(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return;

        var filePath = Path.Combine(_environment.WebRootPath, "uploads", "posts", fileName);

        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePost()
    {
        await LoadCategories();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePost(BlogPost blogPost, IFormFile? image)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(blogPost);
        }

        blogPost.CreatedDate = DateTime.UtcNow;

        if (image is { Length: > 0 })
        {
            var (fileName, error) = await SaveImageAsync(image);

            if (error != null)
            {
                ModelState.AddModelError("image", error);
                await LoadCategories();
                return View(blogPost);
            }

            blogPost.ImageFileName = fileName;
        }

        _context.BlogPosts.Add(blogPost);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Post created successfully!";
        return RedirectToAction("Post", new { id = blogPost.Id });
    }

    public async Task<IActionResult> Posts(string? category)
    {
        var query = _context.BlogPosts.AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        var posts = await query
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();

        await LoadCategories();
        ViewBag.SelectedCategory = category;

        return View(posts);
    }

    public async Task<IActionResult> Post(int id)
    {
        var post = await _context.BlogPosts
            .FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
            return View("PostNotFound");

        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);

        if (post == null)
        {
            TempData["ErrorMessage"] = "Post not found.";
            return RedirectToAction("Posts");
        }

        DeleteImage(post.ImageFileName);
        _context.BlogPosts.Remove(post);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Post '{post.Title}' has been deleted successfully.";
        return RedirectToAction("Posts");
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditPost(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);

        if (post == null)
            return View("PostNotFound");

        await LoadCategories();
        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditPost(int id, BlogPost updatedPost, IFormFile? image, bool removeImage)
    {
        if (id != updatedPost.Id)
            return NotFound();

        if (!ModelState.IsValid)
        {
            await LoadCategories();
            return View(updatedPost);
        }

        var existingPost = await _context.BlogPosts.FindAsync(id);

        if (existingPost == null)
            return NotFound();

        existingPost.Title = updatedPost.Title;
        existingPost.Content = updatedPost.Content;
        existingPost.Category = updatedPost.Category;
        existingPost.ModifiedDate = DateTime.UtcNow;

        if (image is { Length: > 0 })
        {
            var (fileName, error) = await SaveImageAsync(image);

            if (error != null)
            {
                ModelState.AddModelError("image", error);
                updatedPost.ImageFileName = existingPost.ImageFileName;
                await LoadCategories();
                return View(updatedPost);
            }

            DeleteImage(existingPost.ImageFileName);
            existingPost.ImageFileName = fileName;
        }
        else if (removeImage)
        {
            DeleteImage(existingPost.ImageFileName);
            existingPost.ImageFileName = null;
        }

        try
        {
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Post updated successfully!";
            return RedirectToAction("Post", new { id = existingPost.Id });
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "An error occurred while saving changes.");
            await LoadCategories();
            return View(updatedPost);
        }
    }
}