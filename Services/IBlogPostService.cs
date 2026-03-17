using Shellty_Blog.Models;

namespace Shellty_Blog.Services
{
    public interface IBlogPostService
    {
        Task<List<BlogPost>> GetPostsAsync(string? category);
        Task<BlogPost?> GetByIdAsync(int id);
        Task<List<string>> GetCategoriesAsync();
        Task CreateAsync(BlogPost post);
        Task UpdateAsync(BlogPost post);
        Task<bool> DeleteAsync(int id);
        Task<(byte[]? Data, string? ContentType)> GetImageAsync(int id);
        string? ValidateImage(IFormFile file);
        Task ApplyImageAsync(BlogPost post, IFormFile file);
        void RemoveImage(BlogPost post);
    }
}