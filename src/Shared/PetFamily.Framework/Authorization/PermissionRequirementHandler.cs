using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Contracts;
using PetFamily.Core.Models;
using System.IdentityModel.Tokens.Jwt;

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
		using var scope = serviceScopeFactory.CreateAsyncScope();
		var accountContract = scope.ServiceProvider.GetRequiredService<IAccountsContract>();

		var userIdstring = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id)?.Value;

		if (!Guid.TryParse(userIdstring, out var userId) && String.IsNullOrWhiteSpace(userIdstring))
		{
			context.Fail();
			return;
		}

		var permissions = await accountContract.GetUserPermissionCodesAsync(userId, CancellationToken.None);

		if (permissions.Contains(permission.Code))
		{
			context.Succeed(permission);
			return;
		}

		context.Fail();
	}
}
