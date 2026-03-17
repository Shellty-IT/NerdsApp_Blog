using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Models;

namespace Shellty_Blog.Data
{
    public class BlogContext : IdentityDbContext<ApplicationUser>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<AdminRequest> AdminRequests { get; set; }
        public DbSet<AdminApproval> AdminApprovals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminRequest>()
                .HasOne(r => r.User)
                .WithMany(u => u.AdminRequests)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdminApproval>()
                .HasOne(a => a.AdminRequest)
                .WithMany(r => r.Approvals)
                .HasForeignKey(a => a.AdminRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdminApproval>()
                .HasOne(a => a.Admin)
                .WithMany(u => u.AdminApprovals)
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}