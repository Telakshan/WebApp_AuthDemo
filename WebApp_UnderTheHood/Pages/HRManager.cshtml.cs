using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_AuthDemo.Pages;

[Authorize(Policy = "HRManager")]
public class HRManagerModel : PageModel
{
    public void OnGet()
    {
    }
}
