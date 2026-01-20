using Microsoft.AspNetCore.Mvc;
using PurrfectBlog.Data;
using PurrfectBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace PurrfectBlog.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly BlogContext _context;

        public BlogPostController(BlogContext context)
        {
            _context = context;
        }

        // GET: BlogPost/CreatePost
        public IActionResult CreatePost()
        {
            return View();
        }
        
        // POST: BlogPost/CreatePost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.CreatedDate = DateTime.Now;
                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(blogPost);
        }
        public async Task<IActionResult> Posts()
        {
            var posts = await _context.BlogPosts
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
    
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
                ModelState.AddModelError("", "An error occurred while saving changes. Please try again.");
                return View(updatedPost);
            }
        }
    }
}