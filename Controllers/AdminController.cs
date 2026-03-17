using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shellty_Blog.Data;
using Shellty_Blog.Models;

namespace Shellty_Blog.Controllers;

public class AdminController : Controller
{
    private readonly BlogContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(BlogContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> RequestAdmin()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            TempData["ErrorMessage"] = "You are already an administrator.";
            return RedirectToAction("Index", "Home");
        }

        var existingRequest = await _context.AdminRequests
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.Status == AdminRequestStatus.Pending);

        if (existingRequest != null)
        {
            TempData["ErrorMessage"] = "You already have a pending request.";
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestAdmin([FromForm] string? message)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return RedirectToAction("Login", "Account");

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            TempData["ErrorMessage"] = "You are already an administrator.";
            return RedirectToAction("Index", "Home");
        }

        var existingRequest = await _context.AdminRequests
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.Status == AdminRequestStatus.Pending);

        if (existingRequest != null)
        {
            TempData["ErrorMessage"] = "You already have a pending request.";
            return RedirectToAction("Index", "Home");
        }

        var request = new AdminRequest
        {
            UserId = user.Id,
            Message = message ?? string.Empty
        };

        _context.AdminRequests.Add(request);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Your admin request has been submitted. All current admins must approve.";
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Requests()
    {
        var requests = await _context.AdminRequests
            .Include(r => r.User)
            .Include(r => r.Approvals)
                .ThenInclude(a => a.Admin)
            .Where(r => r.Status == AdminRequestStatus.Pending)
            .OrderBy(r => r.CreatedAt)
            .ToListAsync();

        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var totalAdmins = adminRole != null
            ? (await _userManager.GetUsersInRoleAsync("Admin")).Count
            : 0;

        ViewBag.TotalAdmins = totalAdmins;

        return View(requests);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Vote(int requestId, bool approve)
    {
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null)
            return RedirectToAction("Login", "Account");

        var request = await _context.AdminRequests
            .Include(r => r.Approvals)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null || request.Status != AdminRequestStatus.Pending)
        {
            TempData["ErrorMessage"] = "Request not found or already resolved.";
            return RedirectToAction("Requests");
        }

        var existingVote = request.Approvals.FirstOrDefault(a => a.AdminId == currentUser.Id);

        if (existingVote != null)
        {
            TempData["ErrorMessage"] = "You have already voted on this request.";
            return RedirectToAction("Requests");
        }

        var approval = new AdminApproval
        {
            AdminRequestId = requestId,
            AdminId = currentUser.Id,
            IsApproved = approve
        };

        _context.AdminApprovals.Add(approval);
        await _context.SaveChangesAsync();

        if (!approve)
        {
            request.Status = AdminRequestStatus.Rejected;
            request.ResolvedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request has been rejected.";
            return RedirectToAction("Requests");
        }

        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        var totalAdmins = admins.Count;
        var approvalCount = request.Approvals.Count(a => a.IsApproved);

        if (approvalCount >= totalAdmins)
        {
            request.Status = AdminRequestStatus.Approved;
            request.ResolvedAt = DateTime.UtcNow;

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request approved! User is now an admin.";
        }
        else
        {
            TempData["SuccessMessage"] = $"Vote recorded. {approvalCount}/{totalAdmins} approvals.";
        }

        return RedirectToAction("Requests");
    }
    
    [Authorize(Roles = "Admin")]
[HttpGet]
public async Task<IActionResult> Users()
{
    var users = await _userManager.Users
        .OrderBy(u => u.DisplayName)
        .ToListAsync();

    var userRoles = new Dictionary<string, bool>();
    foreach (var user in users)
    {
        userRoles[user.Id] = await _userManager.IsInRoleAsync(user, "Admin");
    }

    ViewBag.UserRoles = userRoles;
    return View(users);
}

[Authorize(Roles = "Admin")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteUser(string userId)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
        return RedirectToAction("Login", "Account");

    if (currentUser.Id == userId)
    {
        TempData["ErrorMessage"] = "You cannot delete your own account.";
        return RedirectToAction("Users");
    }

    var userToDelete = await _userManager.FindByIdAsync(userId);

    if (userToDelete == null)
    {
        TempData["ErrorMessage"] = "User not found.";
        return RedirectToAction("Users");
    }

    var pendingRequests = await _context.AdminRequests
        .Where(r => r.UserId == userId)
        .ToListAsync();

    _context.AdminRequests.RemoveRange(pendingRequests);

    var approvals = await _context.AdminApprovals
        .Where(a => a.AdminId == userId)
        .ToListAsync();

    _context.AdminApprovals.RemoveRange(approvals);

    await _context.SaveChangesAsync();

    var result = await _userManager.DeleteAsync(userToDelete);

    if (result.Succeeded)
    {
        TempData["SuccessMessage"] = $"User '{userToDelete.DisplayName}' has been deleted.";
    }
    else
    {
        TempData["ErrorMessage"] = "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description));
    }

    return RedirectToAction("Users");
}

[Authorize(Roles = "Admin")]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RemoveAdmin(string userId)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
        return RedirectToAction("Login", "Account");

    if (currentUser.Id == userId)
    {
        TempData["ErrorMessage"] = "You cannot remove your own admin role.";
        return RedirectToAction("Users");
    }

    var user = await _userManager.FindByIdAsync(userId);

    if (user == null)
    {
        TempData["ErrorMessage"] = "User not found.";
        return RedirectToAction("Users");
    }

    if (!await _userManager.IsInRoleAsync(user, "Admin"))
    {
        TempData["ErrorMessage"] = "User is not an admin.";
        return RedirectToAction("Users");
    }

    var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

    if (result.Succeeded)
    {
        TempData["SuccessMessage"] = $"Admin role removed from '{user.DisplayName}'.";
    }
    else
    {
        TempData["ErrorMessage"] = "Failed to remove admin role.";
    }

    return RedirectToAction("Users");
}
    
}