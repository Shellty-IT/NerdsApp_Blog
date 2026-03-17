using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shellty_Blog.Models;
using Shellty_Blog.Services;

namespace Shellty_Blog.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly IBlogPostService _blogService;

        public BlogPostController(IBlogPostService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var (data, contentType) = await _blogService.GetImageAsync(id);

            if (data == null || string.IsNullOrEmpty(contentType))
                return NotFound();

            return File(data, contentType);
        }

        public async Task<IActionResult> Posts(string category)
        {
            var posts = await _blogService.GetPostsAsync(category);
            ViewBag.Categories = await _blogService.GetCategoriesAsync();
            ViewBag.SelectedCategory = category;
            return View(posts);
        }

        public async Task<IActionResult> Post(int id)
        {
            var post = await _blogService.GetByIdAsync(id);
            if (post == null) return View("PostNotFound");
            return View(post);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePost()
        {
            ViewBag.Categories = await _blogService.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePost(BlogPost blogPost, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _blogService.GetCategoriesAsync();
                return View(blogPost);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var error = _blogService.ValidateImage(imageFile);
                if (error != null)
                {
                    ModelState.AddModelError("imageFile", error);
                    ViewBag.Categories = await _blogService.GetCategoriesAsync();
                    return View(blogPost);
                }
                await _blogService.ApplyImageAsync(blogPost, imageFile);
            }

            await _blogService.CreateAsync(blogPost);
            TempData["SuccessMessage"] = "Post created successfully!";
            return RedirectToAction("Post", new { id = blogPost.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditPost(int id)
        {
            var post = await _blogService.GetByIdAsync(id);
            if (post == null) return View("PostNotFound");

            ViewBag.Categories = await _blogService.GetCategoriesAsync();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int id, BlogPost updatedPost, IFormFile? imageFile, bool removeImage = false)
        {
            if (id != updatedPost.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _blogService.GetCategoriesAsync();
                return View(updatedPost);
            }

            var existingPost = await _blogService.GetByIdAsync(id);
            if (existingPost == null) return NotFound();

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.Category = updatedPost.Category;

            if (removeImage)
            {
                _blogService.RemoveImage(existingPost);
            }
            else if (imageFile != null && imageFile.Length > 0)
            {
                var error = _blogService.ValidateImage(imageFile);
                if (error != null)
                {
                    ModelState.AddModelError("imageFile", error);
                    ViewBag.Categories = await _blogService.GetCategoriesAsync();
                    return View(updatedPost);
                }
                await _blogService.ApplyImageAsync(existingPost, imageFile);
            }

            try
            {
                await _blogService.UpdateAsync(existingPost);
                TempData["SuccessMessage"] = "Post updated successfully!";
                return RedirectToAction("Post", new { id = existingPost.Id });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while saving changes.");
                ViewBag.Categories = await _blogService.GetCategoriesAsync();
                return View(updatedPost);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var deleted = await _blogService.DeleteAsync(id);

            if (!deleted)
            {
                TempData["ErrorMessage"] = "Post not found.";
            }
            else
            {
                TempData["SuccessMessage"] = "Post has been deleted successfully.";
            }

            return RedirectToAction("Posts");
        }
    }
}