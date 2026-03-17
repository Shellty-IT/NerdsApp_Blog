using System.ComponentModel.DataAnnotations;

namespace Shellty_Blog.Models;

public class AdminApproval
{
    public int Id { get; set; }

    public int AdminRequestId { get; set; }

    public AdminRequest AdminRequest { get; set; } = null!;

    [StringLength(450)]
    public string AdminId { get; set; } = string.Empty;

    public ApplicationUser Admin { get; set; } = null!;

    public bool IsApproved { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}