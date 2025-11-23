using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace health.Pages;

[Authorize]
public abstract class AuthedPageModel(
    Db db,
    UserManager<User> userManager) : PageModel
{
    protected readonly Db Db = db;
    protected readonly UserManager<User> UserManager = userManager;

    private string? _currentUserId;

    protected string CurrentUserId =>
        _currentUserId ??= 
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException(
                "No name identifier claim to get current user ID."
            );


    private User? _currentUser;

    protected async Task<User> GetCurrentUserAsync()
    {
        if (_currentUser is not null)
            return _currentUser;

        _currentUser = await UserManager.FindByIdAsync(CurrentUserId)
            ?? throw new InvalidOperationException(
                $"User with id {CurrentUserId} not found."
            );

        return _currentUser;
    }
}
