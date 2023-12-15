using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_AuthDemo.Pages;

[Authorize(Policy = "MustBelongToHRDepartment")]
public class HumanResourcesModel : PageModel
{
    public void OnGet()
    {
    }
}
