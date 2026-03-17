using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Data;
using Shellty_Blog.Models;

namespace Shellty_Blog.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly BlogContext _context;
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly string[] _allowedTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };

        public BlogPostService(BlogContext context)
        {
            _context = context;
        }

        public async Task<List<BlogPost>> GetPostsAsync(string? category)
        {
            var query = _context.BlogPosts.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            return await query
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new BlogPost
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Category = p.Category,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate,
                    ImageFileName = p.ImageFileName,
                    ImageContentType = p.ImageContentType
                })
                .ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(int id)
        {
            return await _context.BlogPosts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.BlogPosts
                .Where(p => !string.IsNullOrEmpty(p.Category))
                .Select(p => p.Category!)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task CreateAsync(BlogPost post)
        {
            post.CreatedDate = DateTime.Now;
            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BlogPost post)
        {
            post.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null) return false;

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(byte[]? Data, string? ContentType)> GetImageAsync(int id)
        {
            var post = await _context.BlogPosts
                .Where(p => p.Id == id)
                .Select(p => new { p.ImageData, p.ImageContentType })
                .FirstOrDefaultAsync();

            return (post?.ImageData, post?.ImageContentType);
        }

        public string? ValidateImage(IFormFile file)
        {
            if (file.Length > _maxFileSize)
                return "File size cannot exceed 5 MB.";

            if (!_allowedTypes.Contains(file.ContentType.ToLower()))
                return "Only JPEG, PNG, GIF and WebP files are allowed.";

            return null;
        }

        public async Task ApplyImageAsync(BlogPost post, IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            post.ImageData = memoryStream.ToArray();
            post.ImageContentType = file.ContentType;
            post.ImageFileName = file.FileName;
        }

        public void RemoveImage(BlogPost post)
        {
            post.ImageData = null;
            post.ImageContentType = null;
            post.ImageFileName = null;
        }
    }
}