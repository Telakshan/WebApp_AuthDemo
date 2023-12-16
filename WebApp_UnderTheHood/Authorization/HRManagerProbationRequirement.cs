using Microsoft.AspNetCore.Authorization;

namespace WebApp_AuthDemo.Authorization;

public class HRManagerProbationRequirement: IAuthorizationRequirement
{
    public HRManagerProbationRequirement(int probationMonths)
    {
        ProbationMonths = probationMonths;
    }

    public int ProbationMonths { get; set; }
}
 
public class HRManagerProbationRequirementhandler : AuthorizationHandler<HRManagerProbationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
    {
        if(!context.User.HasClaim(x => x.Type.Equals("EmploymentDate", StringComparison.OrdinalIgnoreCase)))
        {
            return Task.CompletedTask;
        }

        if (DateTime.TryParse(context.User.FindFirst(
            x => x.Type.Equals("EmploymentDate", StringComparison.OrdinalIgnoreCase))?.Value, out DateTime employmentDate))
        {
            var period = DateTime.Now - employmentDate;

            if (period.Days > 30 * requirement.ProbationMonths) context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

