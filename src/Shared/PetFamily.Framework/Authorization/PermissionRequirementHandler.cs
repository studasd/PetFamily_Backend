using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace PetFamily.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
	private readonly IServiceScopeFactory serviceScopeFactory;

	public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
	{
		this.serviceScopeFactory = serviceScopeFactory;
	}


	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context, 
		PermissionAttribute permission
	)
	{
		var userPermission = context.User.Claims.FirstOrDefault(x => x.Type == "Permission");
		if (userPermission == null)
			return;

		if (userPermission.Value == permission.Code)
		{
			context.Succeed(permission);
		}
	}
}
