using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.ExternalLogin.Controllers;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    private readonly ILogger<RootController> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;

    public RootController(
        SignInManager<IdentityUser> signInManager, 
        ILogger<RootController> logger)
    {
        _logger = logger;
        _signInManager = signInManager;
    }

    [HttpGet("authenticate", Name = nameof(Authenticate))]
    public async Task<IActionResult> Authenticate()
    {
        var redirectUrl = Url.Action(nameof(ConfirmAuthentication));
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            "Twitter", redirectUrl);

        return Challenge(properties, "Twitter");
    }

    [HttpGet("confirm_authentication", Name = nameof(ConfirmAuthentication))]
    public async Task<IActionResult> ConfirmAuthentication()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        _ = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor:true);

        return RedirectToActionPermanent(nameof(GetUser));
    }

    [Authorize(AuthenticationSchemes = TwitterDefaults.AuthenticationScheme)]
    [HttpGet("get_user", Name = nameof(GetUser))]
    public IActionResult GetUser()
    {
        
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var name = User.FindFirstValue(ClaimTypes.Name);
        return Ok(new { id, name });
    }
}
