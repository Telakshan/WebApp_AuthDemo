using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp_AuthDemo.Common;

namespace WebApp_AuthDemo.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SessionSettings _sessionSettings;
    public LogoutModel(IOptions<SessionSettings> options)
    {
        _sessionSettings = options.Value;
    }

    public async Task<IActionResult> OnPostAsync() 
    {
        await HttpContext.SignOutAsync(_sessionSettings.SessionCookie);
        return RedirectToPage("/Index");
    }
}
