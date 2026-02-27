using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Shellty_Blog.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    public ICollection<AdminRequest> AdminRequests { get; set; } = [];

    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
    public ICollection<AdminApproval> AdminApprovals { get; set; } = [];
}