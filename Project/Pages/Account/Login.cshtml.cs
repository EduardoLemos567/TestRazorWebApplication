using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Project.Models;

namespace Project.Pages.Account;

public class LoginModel : PageModel
{
    private const bool LOCKOUT_ON_FAILURE = false;
    private readonly UserManager<Identity> users;
    private readonly SignInManager<Identity> signor;
    private readonly ILogger<LoginModel> logger;
    private readonly IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor;
    public LoginModel(UserManager<Identity> users,
                      SignInManager<Identity> signor,
                      ILogger<LoginModel> logger,
                      IOptionsMonitor<CookieAuthenticationOptions> optionsMonitor)
    {
        this.users = users;
        this.signor = signor;
        this.logger = logger;
        this.optionsMonitor = optionsMonitor;
    }
    [BindProperty]
    public Login Login { get; set; } = default!;
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            logger.LogDebug("Invalid model state, return to the same page.");
            return Page();
        }
        var identity = await users.FindByEmailAsync(Login.Email!);
        if (identity is null)
        {
            logger.LogDebug("User not found for email {Email}", Login.Email);
            return Unauthorized();
        }
        //TODO: bug, isPersistent is always active between sessions, true or false doesnt change.
        var signinResult = await signor.PasswordSignInAsync(identity!, Login.Password!, Login.RememberMe, LOCKOUT_ON_FAILURE);
        if (!signinResult.Succeeded)
        {
            logger.LogDebug("User {identity} signin failed, reason: {signinResult}.", identity, signinResult);
            return Unauthorized();
        }
        logger.LogDebug("User {identity} logged in.", identity);
        return RedirectToPage("/Index");
    }
    private new IActionResult Unauthorized() => RedirectToPage(
        optionsMonitor
            .Get(IdentityConstants.ApplicationScheme)
            .AccessDeniedPath
    );
}
