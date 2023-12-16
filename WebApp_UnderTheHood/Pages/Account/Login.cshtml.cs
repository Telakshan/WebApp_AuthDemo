using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using WebApp_AuthDemo.Common;
using WebApp_UnderTheHood.Extensions;

namespace WebApp_UnderTheHood.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public Credential Credential { get; set; } = new Credential();
    private readonly SessionSettings _sessionSettings;

    public LoginModel(IOptions<SessionSettings> options)
    {
        _sessionSettings = options.Value;
    }

    private Credential defaultCredentials = new Credential
    {
        UserName = "admin",
        Password = "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk="
    };

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();

        if (Credential.UserName.Equals(defaultCredentials.UserName, StringComparison.OrdinalIgnoreCase) 
            && Credential.Password.Sha256() == defaultCredentials.Password)
        {
            //creating the security context
            var claims = new List<Claim> 
            { 
                new Claim(ClaimTypes.Name, "admin") ,
                new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                new Claim("Department", "HR"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2023-05-01")
            };

            var identity = new ClaimsIdentity(claims, _sessionSettings.SessionCookie);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(_sessionSettings.SessionCookie, claimsPrincipal);

            return RedirectToPage("/Index");
        }

        return Page();
    }
}

public class Credential
{
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
