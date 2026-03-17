using System;
using System.ComponentModel.DataAnnotations;

namespace Shellty_Blog.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? Category { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ImageContentType { get; set; }
        public string? ImageFileName { get; set; }
    }
}