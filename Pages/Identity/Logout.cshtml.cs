namespace health.Pages.Identity;

public class LogoutModel(SignInManager<User> signInManager) : PageModel
{
    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<IActionResult> OnPostAsync()
    {
        await _signInManager.SignOutAsync();
        return LocalRedirect("/");
    }
}
