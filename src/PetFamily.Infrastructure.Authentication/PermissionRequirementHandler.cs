using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Infrastructure.Authentication;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context, 
		PermissionRequirement requirement
	)
	{
		var permission = context.User.Claims.FirstOrDefault(x => x.Type == "Permission");
		if (permission == null)
			return;

		if (permission.Value == requirement.Code)
		{
			context.Succeed(requirement);
		}
	}
}
