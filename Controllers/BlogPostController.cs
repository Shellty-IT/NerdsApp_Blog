using Microsoft.AspNetCore.Mvc;
using ShelltyBlog.Data;
using ShelltyBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace ShelltyBlog.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly BlogContext _context;

        public BlogPostController(BlogContext context)
        {
            _context = context;
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

        public async Task<IActionResult> CreatePost()
        {
            await LoadCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.CreatedDate = DateTime.Now;
                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Post created successfully!";
                return RedirectToAction("Post", new { id = blogPost.Id });
            }
            await LoadCategories();
            return View(blogPost);
        }

        public async Task<IActionResult> Posts(string category)
        {
            var query = _context.BlogPosts.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

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
            {
                return View("PostNotFound");
            }

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);

            if (post == null)
            {
                TempData["ErrorMessage"] = "Post not found.";
                return RedirectToAction("Posts");
            }

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Post '{post.Title}' has been deleted successfully.";
            return RedirectToAction("Posts");
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);

            if (post == null)
            {
                return View("PostNotFound");
            }

            await LoadCategories();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int id, BlogPost updatedPost)
        {
            if (id != updatedPost.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadCategories();
                return View(updatedPost);
            }

            var existingPost = await _context.BlogPosts.FindAsync(id);

            if (existingPost == null)
            {
                return NotFound();
            }

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.Category = updatedPost.Category;
            existingPost.ModifiedDate = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Post updated successfully!";
                return RedirectToAction("Post", new { id = existingPost.Id });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving changes.");
                await LoadCategories();
                return View(updatedPost);
            }
        }
    }
}