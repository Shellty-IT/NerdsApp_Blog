using Microsoft.EntityFrameworkCore;
using PurrfectBlog.Models;

namespace PurrfectBlog.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data - przykładowe posty o kotach
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost
                {
                    Id = 1,
                    Title = "Why Cats Love Boxes",
                    Content = "Cats have an inexplicable love for boxes of all sizes. Whether it's a tiny shoebox or a large cardboard container, if it fits, they sits! Scientists believe this behavior is rooted in their instinct to seek out confined spaces for safety and comfort. Boxes provide cats with a sense of security and a perfect spot for ambushing unsuspecting prey (or your ankles).",
                    Category = "Behavior",
                    CreatedDate = DateTime.Now.AddDays(-10)
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "The Secret Life of Indoor Cats",
                    Content = "Ever wonder what your cat does all day while you're at work? Indoor cats have their own daily routines that might surprise you. From patrolling their territory to taking strategic naps in sunny spots, cats are busy creatures. They spend about 70% of their lives sleeping, which means a 9-year-old cat has been awake for only three years of its life!",
                    Category = "Lifestyle",
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new BlogPost
                {
                    Id = 3,
                    Title = "Understanding Cat Communication",
                    Content = "Cats communicate in many ways beyond meowing. They use body language, purring, and even slow blinks to express themselves. A slow blink from your cat is actually a sign of trust and affection - it's like a kitty kiss! Tail position, ear orientation, and whisker placement all tell a story about how your cat is feeling.",
                    Category = "Behavior",
                    CreatedDate = DateTime.Now.AddDays(-2)
                }
            );
        }
    }
}