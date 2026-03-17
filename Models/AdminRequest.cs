using System.ComponentModel.DataAnnotations;

namespace Shellty_Blog.Models;

public class AdminRequest
{
    public int Id { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;

    [StringLength(500)]
    public string Message { get; set; } = string.Empty;

    public AdminRequestStatus Status { get; set; } = AdminRequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }

    public ICollection<AdminApproval> Approvals { get; set; } = [];
}

public enum AdminRequestStatus
{
    Pending,
    Approved,
    Rejected
}